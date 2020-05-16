using System.Threading.Tasks;

namespace Cognito.Business.Services.Abstract
{
    public interface ITaskService
    {
        Task CopyTaskAsync(int taskId);

        Task CloneTaskAsync(int taskId);
    }
}
