using Cognito.Business.Services.Abstract;
using Cognito.Shared.Exceptions;
using Cognito.Shared.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cognito.Web.BindingModels.Common
{
    public class LocalFilterBindingModel : IValidatableObject
    {
        public int? DomainId { get; set; }

        public int? ProjectId { get; set; }

        public int? TaskId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var Ids = new[] { DomainId, ProjectId, TaskId };

            if (Ids.All(x => !x.HasValue))
            {
                yield return new ValidationResult("At least one of the following filters has to be provided: DomainId, ProjectId or TaskId.");
            }

            if (Ids.Where(id => id.HasValue).Count() > 1)
            {
                yield return new ValidationResult("Just one Id has to be provided!");
            }

            var permissionsService = validationContext.GetRequiredService<IPermissionsService>();

            if (DomainId.HasValue)
            {
                if (AsyncHelper.RunSync(() => permissionsService.HasAccessToDomainAsync(DomainId.Value)))
                {
                    yield return ValidationResult.Success;
                }
                else
                {
                    throw new ForbiddenException();
                }
            }

            if (ProjectId.HasValue)
            {
                if (AsyncHelper.RunSync(() => permissionsService.HasAccessToDomainAsync(ProjectId.Value)))
                {
                    yield return ValidationResult.Success;
                }
                else
                {
                    throw new ForbiddenException();
                }
            }

            if (TaskId.HasValue)
            {
                if (AsyncHelper.RunSync(() => permissionsService.HasAccessToDomainAsync(TaskId.Value)))
                {
                    yield return ValidationResult.Success;
                }
                else
                {
                    throw new ForbiddenException();
                }
            }
        }
    }
}
