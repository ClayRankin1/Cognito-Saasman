using Cognito.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cognito.DataAccess.DbContext.Abstract
{
    public interface ICognitoDbContext : IDisposable
    {
        DbSet<State> States { get; set; }

        DbSet<Address> Addresses { get; set; }

        DbSet<Tenant> Tenants { get; set; }

        DbSet<LicenseType> LicenseTypes { get; set; }

        DbSet<License> Licenses { get; set; }

        DbSet<DomainLicense> DomainLicenses { get; set; }

        DbSet<TimeType> TimeTypes { get; set; }

        DbSet<Time> Times { get; set; }

        DbSet<ProjectTask> Tasks { get; set; }

        DbSet<TaskWebsite> TaskWebsites { get; set; }

        DbSet<TaskContact> TaskContacts { get; set; }

        DbSet<TaskDocument> TaskDocuments { get; set; }

        DbSet<Entities.TaskStatus> TaskStatuses { get; set; }

        DbSet<TaskType> TaskTypes { get; set; }

        DbSet<Subtask> Subtasks { get; set; }

        DbSet<Domain> Domains { get; set; }

        DbSet<UserDomain> UserDomains { get; set; }

        DbSet<DocumentStatus> DocumentStatuses { get; set; }

        DbSet<Document> Documents { get; set; }

        DbSet<Project> Projects { get; set; }

        DbSet<ProjectUser> ProjectUsers { get; set; }

        DbSet<DetailTypeSourceType> DetailTypeSourceTypes { get; set; }

        DbSet<DetailType> DetailTypes { get; set; }

        DbSet<Detail> Details { get; set; }

        DbSet<AccruedTime> AccruedTimes { get; set; }

        DbSet<Point> Points { get; set; }

        DbSet<PointDetail> PointDetails { get; set; }

        DbSet<StatusType> StatusTypes { get; set; }

        DbSet<Contact> Contacts { get; set; }

        DbSet<Website> Websites { get; set; }

        DbSet<Message> Messages { get; set; }

        DbSet<RefreshToken> RefreshTokens { get; set; }

        DbSet<User> Users { get; set; }

        DbSet<UserClaim> UserClaims { get; set; }
        
        DbSet<UserLogin> UserLogins { get; set; }
        
        DbSet<UserToken> UserTokens { get; set; }

        DbSet<UserRole> UserRoles { get; set; }
        
        DbSet<Role> Roles { get; set; }
        
        DbSet<RoleClaim> RoleClaims { get; set; }

        DatabaseFacade Database { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
