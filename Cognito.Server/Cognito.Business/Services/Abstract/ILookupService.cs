using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cognito.Business.Services.Abstract
{
    public interface ILookupService
    {
        Dictionary<string, object> GetAllLookups();
    }
}
