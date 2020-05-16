using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.Shared.Security;
using Cognito.Web.BindingModels;
using Cognito.Web.Controllers.Abstract;
using Cognito.Web.Infrastructure.Attributes;

namespace Cognito.Web.Controllers
{
    [AuthorizeRoles(UserRoles.SysAdmin)]
    public class LicensesController : CrudControllerBase<License, LicenseBindingModel, LicenseViewModel, ILicenseDataService>
    {
        public LicensesController(IMapper mapper, ILicenseDataService dataService) : base(mapper, dataService)
        {
        }
    }
}
