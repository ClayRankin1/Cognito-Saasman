using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Cognito.Web.ResponseModels
{
    public class UserIdentityResult
    {
        public bool Succeeded => !Errors.Any();

        public IList<IdentityError> Errors { get; } = new List<IdentityError>();

        public UserIdentityResult()
        {

        }

        public UserIdentityResult(IdentityResult result)
        {
            Errors = new List<IdentityError>(result.Errors);
        }

        public UserIdentityResult(IEnumerable<IdentityError> errors)
        {
            AddErrors(errors);
        }

        public UserIdentityResult(IdentityResult result, IEnumerable<IdentityError> errors): this(result)
        {
            AddErrors(errors);
        }

        private void AddErrors(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                Errors.Add(error);
            }
        }
    }
}
