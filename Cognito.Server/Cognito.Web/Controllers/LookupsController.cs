using Cognito.Business.Services.Abstract;
using Cognito.Web.Infrastructure.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cognito.Web.Controllers
{
    [AllowAnonymous]
    [CognitoApi]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly IJsonService _jsonService;
        private readonly ILookupService _lookupService;

        public LookupsController(
            IJsonService jsonService,
            ILookupService lookupService)
        {
            _jsonService = jsonService;
            _lookupService = lookupService;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            var result = _lookupService.GetAllLookups();
            return new JsonResult(result, _jsonService.JsonSettings);
        }
    }
}
