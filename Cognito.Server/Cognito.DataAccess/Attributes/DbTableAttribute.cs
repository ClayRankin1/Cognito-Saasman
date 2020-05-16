using Cognito.Shared.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cognito.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableAttribute : TableAttribute
    {
        public DbTableAttribute(string name) : base(name.Pluralize())
        {
        }
    }
}
