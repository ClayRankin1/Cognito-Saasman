using Cognito.DataAccess.DbContext.Abstract;
using System;

namespace Cognito.Business.ViewModels
{
    public class DocumentViewModel : IdentityViewModel, IDateAuditable
    {
        public DateTime DateAdded { get; set; }

        public DateTime DateUpdated { get; set; }

        public string Key { get; set; }

        public string Description { get; set; }

        public string FileName { get; set; }

        public string Url { get; set; }

        public int StatusId { get; set; }
    }
}
