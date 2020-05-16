using Cognito.DataAccess;
using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Services.Abstract;
using Cognito.Shared.Options;
using Cognito.Shared.Security;
using Cognito.Shared.Services.Common.Abstract;
using Cognito.Web.Infrastructure.Exceptions;
using Cognito.Web.Services.Security.Abstract;
using Cognito.Web.Services.Security.Models;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cognito.Web.Services.Security
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string Hs512 = "HS512";
        private const string SigningAlgorithm = SecurityAlgorithms.HmacSha512Signature;

        private readonly ICognitoDbContext _context;
        private readonly IDateTimeProvider _dataTimeProvider;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStoreProcedureRunner _storeProcedureRunner;
        private readonly IOptions<SecurityOptions> _securityOptions;

        public AuthenticationService(
            ICognitoDbContext context,
            IDateTimeProvider dataTimeProvider,
            IPasswordHasher<User> passwordHasher,
            ICurrentUserService currentUserService,
            IStoreProcedureRunner storeProcedureRunner,
            IOptions<SecurityOptions> securityOptions)
        {
            _context = context;
            _dataTimeProvider = dataTimeProvider;
            _passwordHasher = passwordHasher;
            _currentUserService = currentUserService;
            _storeProcedureRunner = storeProcedureRunner;
            _securityOptions = securityOptions;
        }

        public async Task<Tokens> GetTokensAsync(string email, string password)
        {
            var userDetails = await GetUserDetailsAsync(email);
            var result = _passwordHasher.VerifyHashedPassword(userDetails.User, userDetails.User.PasswordHash, password);
            if (result != PasswordVerificationResult.Success)
            {
                throw new InvalidLoginException();
            }

            return await GenerateTokensAsync(userDetails, null);
        }

        public async Task<Tokens> GetRefreshTokenAsync(string accessToken, string refreshToken)
        {
            var userEmail = GetUserEmailFromAccessToken(accessToken);
            var userDetails = await GetUserDetailsAsync(userEmail);

            if (ValidateRefreshToken(userDetails.RefreshTokens, refreshToken))
            {
                return await GenerateTokensAsync(userDetails, refreshToken);
            }

            await UpdateRefreshTokenAsync(userDetails.User.Id, null, null, refreshToken);

            throw new TokenRefreshmentFailedException("Invalid token!");
        }

        private async Task<Tokens> GenerateTokensAsync(UserDetails userDetails, string usedRefreshToken)
        {
            var refreshToken = GenerateJwtRefreshToken();

            await UpdateRefreshTokenAsync(userDetails.User.Id, refreshToken.Token, refreshToken.Expiration, usedRefreshToken);

            return new Tokens
            {
                AccessToken = GenerateJwtAccessToken(userDetails),
                RefreshToken = refreshToken
            };
        }

        private string GenerateJwtAccessToken(UserDetails userDetails)
        {
            // TODO: Once multiple domains are implemented we can remove below block of code!
            var domainId = userDetails.DomainRoles.FirstOrDefault()?.DomainId ?? 0;

            var serializeOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var domainRoles = JsonSerializer.Serialize(userDetails.DomainRoles, serializeOptions);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDetails.User.Id.ToString()),
                new Claim(ClaimTypes.Email, userDetails.User.Email),
                new Claim(CognitoClaimTypes.UserName, userDetails.User.UserName),
                new Claim(CognitoClaimTypes.DomainId, domainId.ToString()),
                new Claim(CognitoClaimTypes.DomainRoles, domainRoles),
            };

            foreach (var role in userDetails.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var credentials = new SigningCredentials(GetSigningKey(), SigningAlgorithm);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = _dataTimeProvider.Now,
                Expires = _dataTimeProvider.Now.AddMinutes(_securityOptions.Value.AccessTokenExpirationInMinutes),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GetUserEmailFromAccessToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                // You might need to validate this one depending on your case
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateActor = false,
                // Do not validate lifetime here
                ValidateLifetime = false,
                IssuerSigningKey = GetSigningKey()
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.SignatureAlgorithm.Equals(Hs512, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token!");
            }

            var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                throw new SecurityTokenException($"Missing claim: {ClaimTypes.Email}!");
            }

            return userEmail;
        }

        public GeneratedRefreshToken GenerateJwtRefreshToken()
        {
            return new GeneratedRefreshToken()
            {
                Token = GenerateRefreshToken(),
                Expiration = _dataTimeProvider.UtcNow.AddMinutes(_securityOptions.Value.RefreshTokenExpirationInMinutes)
            };
        }

        public Task LogoutUserAsync(string refreshToken) => _context.RefreshTokens
            .Where(t => t.UserId == _currentUserService.UserId && t.Token == refreshToken)
            .BatchDeleteAsync();

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private async Task<UserDetails> GetUserDetailsAsync(string email)
        {
            var results = await _storeProcedureRunner.QueryMultipleAsync(StoredProcedures.GetUser, new { Email = email });
            var user = await results.ReadFirstOrDefaultAsync<User>();
            if (user == null)
            {
                throw new InvalidLoginException();
            }

            if (_securityOptions.Value.RequireConfirmedEmail && !(user?.EmailConfirmed ?? false))
            {
                throw new InvalidLoginException("Your e-mail address has to be confirmed.");
            }

            var roles = await results.ReadAsync<Role>();
            var domains = await results.ReadAsync<DomainRole>();
            var refreshTokens = await results.ReadAsync<RefreshToken>();

            return new UserDetails(user, roles, domains, refreshTokens);
        }

        private bool ValidateRefreshToken(IEnumerable<RefreshToken> refreshTokens, string refreshToken)
        {
            var currentRefreshToken = refreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);
            if (currentRefreshToken == null)
            {
                return false;
            }

            // Ensure that the refresh token that we got from storage is not yet expired.
            if (_dataTimeProvider.UtcNow > currentRefreshToken.Expiration)
            {
                return false;
            }

            return true;
        }

        private Task UpdateRefreshTokenAsync(
            int userId,
            string refreshToken,
            DateTime? refreshTokenExpiration,
            string expiredRefreshToken)
        {
            return _storeProcedureRunner.ExecuteAsync(StoredProcedures.UpdateUserRefreshToken, new
            {
                UserId = userId,
                Now = _dataTimeProvider.UtcNow,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = refreshTokenExpiration,
                ExpiredRefreshToken = expiredRefreshToken
            });
        }

        private SymmetricSecurityKey GetSigningKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityOptions.Value.SecurityKey));
    }
}
