using Bogus;
using Cognito.DataAccess.DbContext;
using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Entities;
using Cognito.DataSeeder.Extensions;
using Cognito.DataSeeder.Options;
using Cognito.DataSeeder.Services.Abstract;
using Cognito.Shared.Security;
using Cognito.Shared.Services.Common.Abstract;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.DataSeeder.Services
{
    public class TestDataSeeder : ITestDataSeeder
    {
        private readonly List<int> AddressIds = new List<int>();
        private readonly List<int> TenantIds = new List<int>();
        private readonly List<int> LicenseIds = new List<int>();
        private readonly List<int> DomainIds = new List<int>();
        private readonly List<int> UserIds = new List<int>();
        private readonly List<int> ProjectIds = new List<int>();

        private readonly Random _rnd;
        private readonly DateTime _now;
        private readonly ICognitoDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IOptions<DefaultDataOptions> _dataOptions;
        private readonly IOptions<DataSeedingOptions> _seedingOptions;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public TestDataSeeder(
            ICognitoDbContext context,
            IDateTimeProvider dateTimeProvider,
            IOptions<DefaultDataOptions> dataOptions,
            IOptions<DataSeedingOptions> seedingOptions,
            RoleManager<Role> roleManager,
            UserManager<User> userManager,
            IPasswordHasher<User> passwordHasher)
        {
            _rnd = new Random();
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _dataOptions = dataOptions;
            _seedingOptions = seedingOptions;
            _roleManager = roleManager;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _now = _dateTimeProvider.UtcNow;
        }

        public async Task SeedDataAsync()
        {
            await SeedUsersAndRolesFromConfigurationAsync();
            await SeedTaskTypesAsync();

            if (_seedingOptions.Value.SeedingMode == SeedingMode.Full)
            {
                var sysAdmin = await GetSysAdminUser();

                await SeedRolesAndUsersAsync();
                await SeedAddressesAsync();
                await SeedLicensesAsync(sysAdmin.Id);
                await SeedTenantsAsync(sysAdmin.Id);
                await SeedDomainsAsync(sysAdmin.Id);
                await SeedDomainLicensesAsync();
                await SeedProjectsAsync(sysAdmin.Id);
                await SeedUserDomainsAsync();
                await SeedUserProjectsAsync();
            }
        }

        private Task<User> GetSysAdminUser() => _context.Users
            .Where(u => u.UserRoles.Any(ur => ur.Role.Name == UserRoles.SysAdmin.ToString()))
            .FirstOrDefaultAsync();

        private async Task SeedTaskTypesAsync()
        {
            Log.Information("Seeding Task Types");

            if (_context.TaskTypes.Any())
            {
                return;
            }

            var taskTypes = _dataOptions.Value.TaskTypes
                .FillDateAuditableFields(_now)
                .ToList();

            await SeedCollectionAsync(taskTypes);
        }

        private async Task SeedLicensesAsync(int adminId)
        {
            Log.Information("Seeding Licenses");

            if (_context.Licenses.Any())
            {
                return;
            }

            var licenses = new List<License>();
            foreach (var licenseTypeId in Enum.GetValues(typeof(LicenseTypeId)))
            {
                var licenseFaker = new Faker<License>()
                    .RuleFor(t => t.LicenseTypeId, f => (LicenseTypeId)licenseTypeId)
                    .RuleFor(t => t.Price, f => f.Random.Decimal(0, 1000))
                    .RuleFor(t => t.Name, f => Enum.GetName(typeof(LicenseTypeId), licenseTypeId))
                    .RuleForAuditableFields(adminId);
                licenses.Add(licenseFaker.Generate());
            }

            await SeedCollectionAsync(licenses, ids => LicenseIds.AddRange(ids));
        }

        private async Task SeedRolesAndUsersAsync()
        {
            Log.Information("Seeding Users and Roles");

            if (_context.Users.Count() > _dataOptions.Value.Users.Count())
            {
                return;
            }

            var user = new Faker<User>()
                .RuleFor(t => t.FirstName, f => f.Person.FirstName)
                .RuleFor(t => t.LastName, f => f.Person.LastName)
                .RuleFor(t => t.UserName, f => f.Internet.Email())
                .RuleFor(t => t.NormalizedUserName, (f, u) => u.UserName.ToUpper())
                .RuleFor(t => t.Email, (f, u) => u.UserName)
                .RuleFor(t => t.EmailConfirmed, f => true)
                .RuleFor(t => t.NormalizedEmail, (f, u) => u.Email.ToUpper())
                .RuleFor(t => t.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(t => t.PhoneNumberConfirmed, f => true)
                .RuleFor(t => t.SecurityStamp, f => Guid.NewGuid().ToString("D"))
                .RuleFor(t => t.PasswordHash, (f, u) => _passwordHasher.HashPassword(u, u.Email));

            var users = user.Generate(_dataOptions.Value.RandomUsersCount);

            if (_context is CognitoDbContext context)
            {
                await context.BulkInsertAsync(users, config => 
                {
                    config.SetOutputIdentity = true;
                });

                var userIds = await _context.Users.Select(u => u.Id).ToArrayAsync();
                UserIds.AddRange(userIds);

                var projectTeamMemberRole = await _context.Roles.FirstAsync(role => role.Name == UserRoles.ProjectTeamMember.ToString());
                var userRoles = UserIds
                    .Select(userId => new UserRole
                    {
                        UserId = userId,
                        RoleId = projectTeamMemberRole.Id
                    })
                    .ToList();

                await context.BulkInsertAsync(userRoles);
            }
        }

        private async Task SeedUsersAndRolesFromConfigurationAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var role in Enum.GetNames(typeof(UserRoles)))
            {
                if (roles.Any(r => r.Name == role))
                {
                    continue;
                }

                await _roleManager.CreateAsync(new Role { Name = role });
            }

            var users = await _userManager.Users.ToArrayAsync();

            foreach (var user in _dataOptions.Value.Users)
            {
                if (users.Any(u => u.Email == user.Email))
                {
                    continue;
                }

                var newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.Email,
                    Email = user.Email,
                    EmailConfirmed = true
                };

                var userCreationResult = await _userManager.CreateAsync(newUser, user.Password);
                if (userCreationResult.Succeeded)
                {
                    await _userManager.AddToRolesAsync(newUser, user.Roles);
                }
            }
        }

        private async Task SeedAddressesAsync()
        {
            Log.Information("Seeding Addresses");

            if (_context.Addresses.Any())
            {
                return;
            }

            var address = new Faker<Address>()
                .RuleFor(a => a.Street, f => f.Address.StreetAddress())
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.Zip, f => f.Address.ZipCode())
                .RuleFor(a => a.StateId, f => f.PickRandom<StateId>());

            var addresses = address.Generate(_dataOptions.Value.AddressesCount);

            await SeedCollectionAsync(addresses, (ids) => AddressIds.AddRange(ids));
        }

        private async Task SeedTenantsAsync(int adminId)
        {
            Log.Information("Seeding Tenants");

            if (_context.Tenants.Any())
            {
                return;
            }

            var tenant = new Faker<Tenant>()
                .RuleFor(t => t.CompanyName, f => f.Company.CompanyName())
                .RuleFor(t => t.AddressId, f => f.PickRandom(AddressIds))
                .RuleFor(t => t.ContactName, f => f.Name.FullName())
                .RuleFor(t => t.Email, f => f.Internet.Email())
                .RuleFor(t => t.Phone, f => f.Phone.PhoneNumber())
                .RuleForAuditableFields(adminId);

            var tenants = tenant.Generate(_dataOptions.Value.TenantsCount);

            await SeedCollectionAsync(tenants, ids => TenantIds.AddRange(ids));
        }

        private async Task SeedDomainsAsync(int adminId)
        {
            Log.Information("Seeding Domains");

            if (_context.Domains.Count() > 1)
            {
                return;
            }

            var domains = new List<Domain>();

            foreach (var tenantId in TenantIds)
            {
                var domain = new Faker<Domain>()
                    .RuleFor(t => t.TenantId, f => tenantId)
                    .RuleFor(t => t.AdminUserId, f => adminId)
                    .RuleFor(t => t.Name, f => f.Commerce.Department())
                    .RuleForAuditableFields(adminId);

                var tenantDomains = domain.Generate(_rnd.Next(_dataOptions.Value.DomainsMinCountPerTenant, _dataOptions.Value.DomainsMaxCountPerTenant));
                domains.AddRange(tenantDomains);
            }

            await SeedCollectionAsync(domains, ids => DomainIds.AddRange(ids));
        }

        private async Task SeedDomainLicensesAsync()
        {
            Log.Information("Seeding Domains Licenses");

            if (_context.DomainLicenses.Any())
            {
                return;
            }

            var domainIds = DomainIds;

            if (_context.Domains.Any())
            {
                domainIds = await _context.Domains.Select(d => d.Id).ToListAsync();
            }

            var domainLicenses = new List<DomainLicense>();
            foreach (var domainId in domainIds)
            {
                foreach (var licenseId in LicenseIds)
                {
                    var domainLicense = new Faker<DomainLicense>()
                        .RuleFor(t => t.DomainId, f => domainId)
                        .RuleFor(t => t.LicenseId, f => licenseId)
                        .RuleFor(t => t.Licenses, f => f.Random.Int(1, 10000))
                        .RuleFor(t => t.Discount, f => f.Random.Int(1, 50))
                        .RuleFor(t => t.Price, f => f.Random.Decimal(1, 1000));
                    domainLicenses.Add(domainLicense.Generate());
                }
            }

            if (_context is CognitoDbContext context)
            {
                await context.BulkInsertOrUpdateOrDeleteAsync(domainLicenses, config =>
                {
                    config.SetOutputIdentity = true;
                });
            }
        }

        private async Task SeedProjectsAsync(int adminId)
        {
            Log.Information("Seeding Projects");

            if (_context.Projects.Any())
            {
                return;
            }

            var projects = new List<Project>();

            foreach (var domainId in DomainIds)
            {
                var domain = new Faker<Project>()
                    .RuleFor(t => t.FullName, f => f.Lorem.Sentence())
                    .RuleFor(t => t.Nickname, f => f.Lorem.Word())
                    .RuleFor(t => t.ProjectNo, f => f.Random.AlphaNumeric(8))
                    .RuleFor(t => t.ClientNo, f => f.Random.AlphaNumeric(8))
                    .RuleFor(t => t.DomainId, f => domainId)
                    .RuleFor(t => t.OwnerId, f => adminId)
                    .RuleForAuditableFields(adminId);

                var domainProjects = domain.Generate(_rnd.Next(_dataOptions.Value.ProjectsMinCountPerDomain, _dataOptions.Value.ProjectsMaxCountPerDomain));
                projects.AddRange(domainProjects);
            }

            await SeedCollectionAsync(projects, ids => ProjectIds.AddRange(ids));
        }

        private async Task SeedUserDomainsAsync()
        {
            Log.Information("Seeding User-Domain relations");

            if (_context.UserDomains.Any())
            {
                return;
            }

            var userRolesIds = await _roleManager.Roles.Select(r => r.Id).ToListAsync();
            var userDomains = new List<UserDomain>();

            foreach (var domainId in DomainIds)
            {
                var userDomain = new Faker<UserDomain>()
                    .RuleFor(t => t.DomainId, f => domainId)
                    .RuleFor(t => t.UserId, f => f.PickRandom(UserIds))
                    .RuleFor(t => t.RoleId, f => (int)UserRoles.Basic);

                userDomains.AddRange(
                    userDomain.Generate(_rnd.Next(_dataOptions.Value.UserDomainsMinCountPerDomain, _dataOptions.Value.UserDomainsMaxCountPerDomain))
                );
            }

            // remove duplicities - we can have same UserId and DomainId records => random generation
            userDomains = userDomains
                .GroupBy(up => new { up.UserId, up.DomainId, up.RoleId })
                .Select(g => new UserDomain
                {
                    UserId = g.Key.UserId,
                    DomainId = g.Key.DomainId,
                    RoleId = g.Key.RoleId
                })
                .ToList();

            if (_context is CognitoDbContext context)
            {
                await context.BulkInsertOrUpdateOrDeleteAsync(userDomains);
            }
        }

        private async Task SeedUserProjectsAsync()
        {
            Log.Information("Seeding User-Project relations");

            if (_context.ProjectUsers.Any())
            {
                return;
            }

            var userProjects = new List<ProjectUser>();

            var data = await _context.UserDomains
                .Join(_context.Projects, ud => ud.DomainId, p => p.DomainId, (ud, p) => new
                {
                    UserDomain = ud,
                    Project = p
                })
                .Select(x => new
                {
                    x.UserDomain.DomainId,
                    x.UserDomain.UserId,
                    ProjectId = x.Project.Id
                })
                .ToArrayAsync();

            var usersGrouppedByDomainId = data
                .GroupBy(x => new { x.ProjectId })
                .Select(g => new 
                {
                    g.Key.ProjectId,
                    UserIds = g.Select(x => x.UserId)
                })
                .ToArray();

            foreach (var usersInDomain in usersGrouppedByDomainId)
            {
                var userProject = new Faker<ProjectUser>()
                    .RuleFor(t => t.UserId, f => f.PickRandom(usersInDomain.UserIds))
                    .RuleFor(t => t.ProjectId, f => usersInDomain.ProjectId);

                userProjects.AddRange(
                    userProject.Generate(_rnd.Next(_dataOptions.Value.UserProjectsMinCountPerDomain, _dataOptions.Value.UserProjectsMaxCountPerDomain))
                );
            }

            // remove duplicities - we can have same UserId and ProjectId records => random generation
            userProjects = userProjects
                .GroupBy(up => new { up.UserId, up.ProjectId })
                .Select(g => new ProjectUser 
                {
                    UserId = g.Key.UserId,
                    ProjectId = g.Key.ProjectId
                })
                .ToList();

            if (_context is CognitoDbContext context)
            {
                await context.BulkInsertAsync(userProjects);
            }
        }

        private async Task SeedCollectionAsync<TEntity>(IList<TEntity> entities, Action<IEnumerable<int>> idCallback = null) where TEntity : class, IEntity
        {
            if (_context is CognitoDbContext context)
            {
                await context.BulkInsertOrUpdateOrDeleteAsync(entities, config =>
                {
                    config.SetOutputIdentity = true;
                });

                idCallback?.Invoke(entities.Select(e => e.Id));
            }
        }
    }
}
