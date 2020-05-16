using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.DataStructures;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices
{
    public class PointDataService : DataServiceBase<Point, PointViewModel, IPointRepository>, IPointDataService
    {
        public PointDataService(IMapper mapper, IPointRepository repository, IDateTimeProvider dateTimeProvider, ICurrentUserService currentUserService) : base(mapper, repository, dateTimeProvider, currentUserService)
        {
        }

        public async Task<int> GetCountAsync(int projectId, int? parentId)
        {
            return await _repository.GetCountAsync(projectId, parentId);
        }

        public async Task<PointViewModel> GetPointDetailsByPointIdAsync(int id)
        {
            var point = await _repository.GetPointDetailsByPointIdAsync(id);

            return _mapper.Map<PointViewModel>(point);
        }

        public async Task<IEnumerable<PointViewModel>> GetPointDetailsByDetailIdsAsync(IEnumerable<int> ids)
        {
            return (from point in await _repository.GetPointDetailsByDetailIdsAsync(ids) select _mapper.Map<PointViewModel>(point)).ToList();
        }

        public async Task<PointViewModel> AddPointDetailsAsync(int id, IEnumerable<int> detailIds)
        {
            var point = await _repository.GetByIdAsync(id);

            foreach (var detailId in detailIds)
            {
                if (point.PointDetails.Any(pd => pd.DetailId == detailId)) continue;

                var pointDetail = new PointDetail() {DetailId = detailId, PointId = id};
                
                point.PointDetails.Add(pointDetail);
            }

            await _repository.SaveAllAsync();

            return _mapper.Map<PointViewModel>(point);
        }

        public async Task<IEnumerable<PointViewModel>> GetByProjectIdAsync(int projectId)
        {
            var points = await _repository.GetByProjectIdAsync(projectId);
            
            var pointsToReturn = new List<PointViewModel>();
            
            foreach (var point in points)
            {
                if (!points.Any(p => p.Id == point.ParentId))
                {
                    // DevExtreme will ignore points that have a parent id if the parent is
                    // not returned along with the child. As a workaround, we can send back
                    // the point with a null ParentId
                    point.ParentId = null;
                }
                pointsToReturn.Add(_mapper.Map<PointViewModel>(point));
            }
            
            return pointsToReturn;
        }

        public async Task<PointViewModel> ReorderAsync(Point point)
        {
            // TODO: Cache tree?
            var tree = await BuildTreeAsync(point);
            
            RefreshTreeAtNode(tree.Root);

            return _mapper.Map<PointViewModel>(tree.Find(point).Value);
        }

        private void RefreshTreeAtNode(TreeNode<Point> node)
        {
            var count = 0;
            
            foreach (var child in node.Children)
            {
                child.Value.Count = ++count;

                if (child.Value.ParentId != null)
                {
                    child.Value.Label = node.Value.Label + "." + child.Value.Count;
                }
                else
                {
                    child.Value.Label = child.Value.Count.ToString();
                }

                _repository.Update(child.Value);

                RefreshTreeAtNode(child);
            }
        }

        private async Task<Tree<Point>> BuildTreeAsync(Point pointToReorder)
        {
            var points = await _repository.GetAll()
                .Where(p => p.ProjectId == pointToReorder.ProjectId)
                .OrderBy(o => o.Count)
                .ToListAsync();

            var tree = new Tree<Point>(new Point());
            
            while (tree.Count != points.Count() + 1)
            {
                foreach (var point in points)
                {
                    var node = tree.Find(point);
                    if (point.Id == pointToReorder.Id || node != null)
                    {
                        if (pointToReorder.ParentId == point.Id && point.Children.Count == pointToReorder.Count - 1)
                        {
                            tree.AddNode(new TreeNode<Point>(pointToReorder, node));
                        }
                        continue;
                    }
                    
                    if (point.ParentId == null)
                    {
                        if (pointToReorder.ParentId == null && tree.Root.Children.Count == pointToReorder.Count - 1)
                        {
                            tree.AddNode(new TreeNode<Point>(pointToReorder, tree.Root));
                        }

                        tree.AddNode(new TreeNode<Point>(point, tree.Root));
                        continue;
                    }

                    var parent = tree.Find(point.Parent);
                    if (parent != null)
                    {
                        if (pointToReorder.ParentId == parent.Value.Id &&
                            parent.Children.Count == pointToReorder.Count - 1)
                        {
                            tree.AddNode(new TreeNode<Point>(pointToReorder, parent));
                        }

                        tree.AddNode(new TreeNode<Point>(point, parent));
                    }
                }
            }

            return tree;
        }
    }
}