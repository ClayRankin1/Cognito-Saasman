using Newtonsoft.Json;

namespace Cognito.Business.Services.Abstract
{
    public interface IJsonService
    {
        JsonSerializerSettings JsonSettings { get; }

        string SerializeObject(object obj);
    }
}
