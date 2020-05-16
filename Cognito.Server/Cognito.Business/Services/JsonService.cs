using Cognito.Business.Services.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cognito.Business.Services
{
    public class JsonService : IJsonService
    {
        public JsonSerializerSettings JsonSettings => new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        public string SerializeObject(object obj) => JsonConvert.SerializeObject(obj, JsonSettings);
    }
}
