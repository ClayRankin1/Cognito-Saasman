using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Cognito.DataAccess.Repositories
{
    public class PointRepository : CrudRepository<Point>, IPointRepository
    {
        public PointRepository(ICognitoDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
        }

        public override async void Add(Point entity)
        {
            entity.Count = await GetCountAsync(entity.ProjectId, entity.ParentId) + 1;

            if (entity.ParentId != null)
            {
                var parent = await GetAll().Where(p => p.Id == entity.ParentId).FirstOrDefaultAsync();
                entity.Label = parent == null ? entity.Count.ToString() : $"{parent.Label}.{entity.Count}";
            }
            else
            {
                entity.Label = entity.Count.ToString();
            }

            base.Add(entity);
        }

        public async Task<int> GetCountAsync(int projectId, int? parentId)
        {
            return await GetAll()
                .Where(p => p.ProjectId == projectId && p.ParentId == parentId)
                .CountAsync();
        }

        public async Task<Point> GetPointDetailsByPointIdAsync(int id)
        {
            return await GetAll()
                .Include(p => p.PointDetails)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Point>> GetPointDetailsByDetailIdsAsync(IEnumerable<int> ids)
        {
            return await _context.PointDetails
                .Include(pd => pd.Point)
                .Where(pd => ids.Contains(pd.DetailId))
                .Select(pd => pd.Point)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<Point>> GetByProjectIdAsync(int projectId)
        {
            return await GetAll()
                .Where(p => p.ProjectId == projectId)
                .ToListAsync();
        }
    }
}