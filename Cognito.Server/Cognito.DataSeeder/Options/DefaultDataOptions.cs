using Cognito.DataAccess.Entities;

namespace Cognito.DataSeeder.Options
{
    public class DefaultDataOptions
    {
        public UserOptions[] Users { get; set; }

        public int RandomUsersCount { get; set; }

        public int AddressesCount { get; set; }

        public int TenantsCount { get; set; }

        public int DomainsMinCountPerTenant { get; set; }

        public int DomainsMaxCountPerTenant { get; set; }

        public int ProjectsMinCountPerDomain { get; set; }

        public int ProjectsMaxCountPerDomain { get; set; }

        public int UserDomainsMinCountPerDomain { get; set; }

        public int UserDomainsMaxCountPerDomain { get; set; }

        public int UserProjectsMinCountPerDomain { get; set; }

        public int UserProjectsMaxCountPerDomain { get; set; }

        public TaskType[] TaskTypes { get; set; }
    }
}
