﻿using Cognito.Business.Services.Abstract;
using Cognito.Shared.Exceptions;
using Cognito.Shared.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.Infrastructure.Attributes
{
    public sealed class TaskAccessPermissionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is int?))
            {
                return ValidationResult.Success;
            }

            var taskId = (int)value;
            var permissionsService = validationContext.GetRequiredService<IPermissionsService>();
            if (AsyncHelper.RunSync(() => permissionsService.HasAccessToTaskAsync(taskId)))
            {
                return ValidationResult.Success;
            }

            throw new ForbiddenException();
        }
    }
}