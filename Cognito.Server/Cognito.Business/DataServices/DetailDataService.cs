using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.Exceptions;
using Cognito.Business.Services.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Extensions;
using Cognito.Shared.Services.Common.Abstract;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices
{
    public class DetailDataService : DataServiceBase<Detail, DetailViewModel, IDetailRepository>, IDetailDataService
    {
        private const char CaretChar = '^';
        private readonly DetailTypeId[] NonMergableItemTypes =
        {
            DetailTypeId.DocReference,
            DetailTypeId.ContactReference,
            DetailTypeId.Quote,
            DetailTypeId.WebReference,
            DetailTypeId.Email
        };

        private readonly IPermissionsService _permissionsService;

        public DetailDataService(
            IMapper mapper,
            IDetailRepository repository,
            IDateTimeProvider dateTimeProvider,
            ICurrentUserService currentUserService,
            IPermissionsService permissionsService) : base(mapper, repository, dateTimeProvider, currentUserService)
        {
            _permissionsService = permissionsService;
        }

        public Task<DetailViewModel[]> GetDetailsByTaskIdAsync(int taskId, int? detailTypeId)
        {
            var query = _repository
                .GetAll()
                .Where(d => d.TaskId == taskId);

            if (detailTypeId.HasValue)
            {
                query = query.Where(d => d.DetailTypeId == (DetailTypeId)detailTypeId.Value);
            }

            return query
                .ProjectTo<DetailViewModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public Task<PointDetailViewModel[]> GetLinkedDetailsAsync(int projectId)
        {
            return _repository
                .GetAll()
                .Where(d => d.Task.ProjectId == projectId)
                .SelectMany(d => d.PointDetails)
                .ProjectTo<PointDetailViewModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task SplitAsync(int[] detailIds)
        {
            if (!detailIds?.Any() ?? true)
            {
                throw new ClientInvalidOperationException("No Details are selected.");
            }

            var details = await _repository
                .GetAll()
                .Include(d => d.DetailType)
                .Where(d => detailIds.Contains(d.Id))
                .ToArrayAsync();

            var unsplittableItems = details
                .Where(i => NonMergableItemTypes.Contains(i.DetailType.Id))
                .Select(i => i.DetailType.Label)
                .Distinct();

            if (unsplittableItems.Any())
            {
                var message = $"{string.Join(", ", unsplittableItems.Select(i => i.Pluralize()))} cannot be split.";
                throw new ClientInvalidOperationException(message, "Cannot be split");
            }

            foreach (var detail in details)
            {
                if (!detail.Body.Contains(CaretChar))
                {
                    continue;
                }

                var bodyParts = detail.Body?.Split(CaretChar, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
                foreach (var bodyPart in bodyParts)
                {
                    var clonedDetail = _repository.Clone(detail);
                    clonedDetail.Id = 0;
                    clonedDetail.Body = bodyPart;
                    _repository.Add(clonedDetail);
                }

                _repository.DeleteById(detail.Id);
            }

            await _repository.SaveAllAsync();
        }

        public async Task MergeAsync(int[] detailIds)
        {
            if (!detailIds.Any() || detailIds.Count() == 1)
            {
                throw new ClientInvalidOperationException("No Details to be merged.", "Error");
            }

            var details = await _repository
                .GetAll()
                .AsNoTracking()
                .Where(d => detailIds.Contains(d.Id))
                .Include(d => d.DetailType)
                .OrderBy(d => d.Id)
                .ToArrayAsync();

            // validate all Details are the same type
            if (details.GroupBy(d => d.DetailTypeId).Count() > 1)
            {
                throw new ClientInvalidOperationException("Only details that are the same type can be merged. Please change your selection.", "Cannot be merged");
            }

            var nonMergableItems = details
                .Where(i => NonMergableItemTypes.Contains(i.DetailTypeId))
                .Select(i => i.DetailType.Label)
                .Distinct();

            if (nonMergableItems.Any())
            {
                var message = $"{string.Join(", ", nonMergableItems.Select(i => i.Pluralize()))} cannot be merged.";
                throw new ClientInvalidOperationException(message, "Cannot be merged");
            }

            var mergedDetailBodies = details
                .Select(d => d.Body?.Trim())
                .ToArray();

            var mergedDetail = _repository.Clone(details.First());
            mergedDetail.Id = 0;
            mergedDetail.Body = string.Join(Environment.NewLine, mergedDetailBodies);

            _repository.Add(mergedDetail);

            foreach (var detail in details)
            {
                _repository.DeleteById(detail.Id);
            }

            await _repository.SaveAllAsync();
        }

        public async Task<int> BulkDeleteDetailsAsync(IEnumerable<int> ids)
        {
            await _permissionsService.EnsureDetailsAccessAsync(ids);

            return await _repository
                .GetAll()
                .Where(d => ids.Contains(d.Id))
                .BatchUpdateAsync(new Detail 
                {
                    UpdatedByUserId = _currentUserService.UserId,
                    DateUpdated = _dateTimeProvider.UtcNow,
                    IsDeleted = true 
                });
        }

        public async Task<DetailViewModel> CopyDetailAsync(int detailId, int targetTaskId)
        {
            await _permissionsService.EnsureTaskAccessAsync(targetTaskId);

            var detail = await _repository.GetByIdAsync(detailId);
            if (detail == null)
            {
                throw new EntityNotFoundException($"The Detail with Id: {detailId} was not found.");
            }

            var copiedDetail = _repository.Clone(detail);
            copiedDetail.TaskId = targetTaskId;

            return await CreateAsync(copiedDetail);
        }
    }
}
