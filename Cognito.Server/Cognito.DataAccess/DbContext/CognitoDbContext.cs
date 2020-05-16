using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Extensions;
using Cognito.Shared.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;

namespace Cognito.DataAccess.DbContext
{
    public class CognitoDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>, ICognitoDbContext
    {
        public DbSet<State> States { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<LicenseType> LicenseTypes { get; set; }

        public DbSet<License> Licenses { get; set; }

        public DbSet<DomainLicense> DomainLicenses { get; set; }

        public DbSet<TimeType> TimeTypes { get; set; }

        public DbSet<Time> Times { get; set; }

        public DbSet<ProjectTask> Tasks { get; set; }

        public DbSet<TaskWebsite> TaskWebsites { get; set; }

        public DbSet<TaskContact> TaskContacts { get; set; }

        public DbSet<TaskDocument> TaskDocuments { get; set; }

        public DbSet<TaskStatus> TaskStatuses { get; set; }

        public DbSet<TaskType> TaskTypes { get; set; }

        public DbSet<Subtask> Subtasks { get; set; }

        public DbSet<Domain> Domains { get; set; }

        public DbSet<UserDomain> UserDomains { get; set; }

        public DbSet<DocumentStatus> DocumentStatuses { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectUser> ProjectUsers { get; set; }

        public DbSet<DetailTypeSourceType> DetailTypeSourceTypes { get; set; }

        public DbSet<DetailType> DetailTypes { get; set; }

        public DbSet<Detail> Details { get; set; }

        public DbSet<AccruedTime> AccruedTimes { get; set; }

        public DbSet<Point> Points { get; set; }

        public DbSet<PointDetail> PointDetails { get; set; }

        public DbSet<StatusType> StatusTypes { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Website> Websites { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public CognitoDbContext(DbContextOptions<CognitoDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.DisableCascadeDeletes();

            var setGlobalQueryMethod = GetType()
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

            // Apply the Global Query Filter (e.IsDeleted == false) to every EntityBase entity
            builder.Model
                .GetEntityTypes()
                .ForEach(entityType =>
                {
                    if (typeof(DeletableEntityBase).IsAssignableFrom(entityType.ClrType))
                    {
                        var method = setGlobalQueryMethod.MakeGenericMethod(entityType.ClrType);
                        method.Invoke(this, new object[] { builder });
                    }
                });

            // apply all the configurations from current assembly
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public static void SetGlobalQuery<T>(ModelBuilder builder) where T : DeletableEntityBase
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
