using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Extensions;
using Cognito.DataAccess.Repositories.Results;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cognito.DataAccess.Repositories.Abstract
{
    public class CrudRepository<TEntity> : ICrudRepository<TEntity> where TEntity : class, IEntity, new()
    {
        protected readonly ICognitoDbContext _context;
        protected readonly ICurrentUserService _currentUserService;

        public CrudRepository(
            ICognitoDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        protected virtual Expression<Func<TEntity, bool>> GetAllPredicate { get; } = null;

        protected virtual IQueryable<TEntity> GetAllInternal() => _context.Set<TEntity>().AsQueryable();

        public virtual IQueryable<TEntity> GetAll()
        {
            if (GetAllPredicate == null)
            {
                return GetAllInternal().AsNoTracking();
            }

            return GetAllInternal().AsNoTracking().Where(GetAllPredicate);
        }

        public virtual Task<TEntity> GetByIdAsync(int id)
        {
            return GetAll().SingleOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<PaginatedList<TEntity>> GetPageAsync(
            int pageNumber,
            int pageSize,
            string sortField,
            string sortOrder)
        {
            var entityList = GetAll().OrderByDynamic(sortField, sortOrder.ToUpper());
            return await PaginatedList<TEntity>.CreateAsync(entityList, pageNumber, pageSize);
        }

        public virtual void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);

            if (entity is IDateAuditable dateAuditable)
            {
                _context.Entry(dateAuditable).Property(e => e.DateAdded).IsModified = false;
            }

            if (entity is IAuditable auditableEntity)
            {
                _context.Entry(auditableEntity).Property(e => e.CreatedByUserId).IsModified = false;
            }
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public virtual void DeleteById(int id)
        {
            var entityToDelete = new TEntity { Id = id, IsDeleted = true };
            var entry = _context.Entry(entityToDelete);

            _context.Set<TEntity>().Update(entityToDelete);

            foreach (var property in entry.Properties)
            {
                property.IsModified = false;
            }

            entry.Property(p => p.IsDeleted).IsModified = true;
        }

        public virtual TEntity Clone(TEntity entity)
        {
            var clonedEntity = _context.Entry(entity).CurrentValues.ToObject() as TEntity;
            var IdName = nameof(entity.Id);
            if (clonedEntity.GetType().GetProperty(IdName) != null)
            {
                clonedEntity.GetType().GetProperty(IdName).SetValue(clonedEntity, 0);
            }

            return clonedEntity;
        }

        public virtual async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
