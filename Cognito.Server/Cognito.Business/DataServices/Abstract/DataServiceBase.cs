using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cognito.Business.Exceptions;
using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Extensions;
using Cognito.DataAccess.Filtering;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.DataAccess.Repositories.Results;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices.Abstract
{
    public abstract class DataServiceBase<TEntity, TOutput, TRepository> : IDataService<TEntity, TOutput>
        where TEntity : class, IEntity, new()
        where TOutput : class, new()
        where TRepository : class, ICrudRepository<TEntity>
    {
        protected readonly IMapper _mapper;
        protected readonly TRepository _repository;
        protected readonly IDateTimeProvider _dateTimeProvider;
        protected readonly ICurrentUserService _currentUserService;

        protected DataServiceBase(
            IMapper mapper,
            TRepository repository,
            IDateTimeProvider dateTimeProvider,
            ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _repository = repository;
            _dateTimeProvider = dateTimeProvider;
            _currentUserService = currentUserService;
        }

        public virtual async Task<TOutput[]> GetAllAsync()
        {
            var entities = await _repository.GetAll().ToArrayAsync();
            return _mapper.Map<TOutput[]>(entities);
        }

        public virtual async Task<TOutput> GetAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException($"The {typeof(TEntity).Name} with Id: {id} was not found.");
            }
            
            return _mapper.Map<TOutput>(entity);
        }

        public virtual async Task<PaginatedList<TOutput>> FilterAsync(DataFilter filter)
        {
            var query = _repository
                .GetAll()
                .ApplyFilters(filter.Filter)
                .ApplySorting(filter.Sorts)
                .ProjectTo<TOutput>(_mapper.ConfigurationProvider);

            return await PaginatedList<TOutput>.CreateAsync(query, filter.Paging.PageNumber, filter.Paging.PageSize);
        }

        public virtual async Task<TOutput> CreateAsync(TEntity entity)
        {
            FillAuditableProperties(entity, isCreatingNewEntity: true);

            _repository.Add(entity);
            await _repository.SaveAllAsync();

            return _mapper.Map<TOutput>(entity);
        }

        public virtual async Task<TOutput> UpdateAsync(TEntity entity)
        {
            FillAuditableProperties(entity, isCreatingNewEntity: false);

            _repository.Update(entity);
            await _repository.SaveAllAsync();

            return _mapper.Map<TOutput>(entity);
        }

        public virtual Task DeleteAsync(int id)
        {
            _repository.DeleteById(id);
            return _repository.SaveAllAsync();
        }

        protected void FillAuditableProperties(TEntity entity, bool isCreatingNewEntity)
        {
            if (entity is IDateAuditable dateAuditable)
            {
                var currentUtcTime = _dateTimeProvider.UtcNow;

                if (isCreatingNewEntity)
                {
                    dateAuditable.DateAdded = currentUtcTime;
                }
                
                dateAuditable.DateUpdated = currentUtcTime;
            }

            if (entity is IAuditable auditable)
            {
                if (isCreatingNewEntity)
                {
                    auditable.CreatedByUserId = _currentUserService.UserId;
                }

                auditable.UpdatedByUserId = _currentUserService.UserId;
            }
        }
    }
}
