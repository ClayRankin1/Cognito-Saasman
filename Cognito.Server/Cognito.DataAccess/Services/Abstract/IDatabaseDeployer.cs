using System.Threading.Tasks;

namespace Cognito.DataAccess.Services.Abstract
{
    public interface IDatabaseDeployer
    {
        Task DeployChangeScriptsAsync();
    }
}
