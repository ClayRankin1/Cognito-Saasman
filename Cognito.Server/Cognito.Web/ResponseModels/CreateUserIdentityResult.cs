using Cognito.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cognito.Web.ResponseModels
{
    public class CreateUserIdentityResult : UserIdentityResult
    {
        public User User { get; }

        public CreateUserIdentityResult(IdentityResult result, User user): base(result)
        {
            User = user;
        }
    }
}
