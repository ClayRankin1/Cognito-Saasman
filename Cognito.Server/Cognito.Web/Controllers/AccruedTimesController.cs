using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.Web.BindingModels;
using Cognito.Web.Controllers.Abstract;

namespace Cognito.Web.Controllers
{
    public class AccruedTimesController : CrudControllerBase<AccruedTime, AccruedTimeBindingModel, AccruedTimeViewModel, IAccruedTimeDataService>
    {
        public AccruedTimesController(
            IMapper mapper,
            IAccruedTimeDataService dataService) : base(mapper, dataService)
        {
        }
    }
}
