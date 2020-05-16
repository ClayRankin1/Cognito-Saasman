using System;

namespace Cognito.DataAccess.DbContext.Abstract
{
    public interface IDateAuditable
    {
        DateTime DateAdded { get; set; }

        DateTime DateUpdated { get; set; }
    }
}
