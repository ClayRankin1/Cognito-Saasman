using System;
using System.Collections.Generic;

namespace Cognito.Business.ViewModels
{
    public class ProjectViewModel : AuditableViewModel
    {
        public string FullName { get; set; }

        public string Nickname { get; set; }

        public string ProjectNo { get; set; }

        public string Description { get; set; }

        public int ProjectTypeId { get; set; }
        public bool IsBillable { get; set; }

        public int OwnerId { get; set; }

        public UserViewModel Owner { get; set; }

        public int? ProxyId { get; set; }

        public UserViewModel Proxy { get; set; }

        public string ClientNo { get; set; }

        public DateTime? ArchivedOn { get; set; }

        public int DomainId { get; set; }

        public List<ProjectUserViewModel> Users { get; set; }
    }
}
