using Cognito.DataAccess.Services.Abstract;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Cognito.Web.Infrastructure.Filters
{
    public sealed class TransactionScopeAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var transactionProvider = context.HttpContext.RequestServices.GetRequiredService<ITransactionProvider>();
            transactionProvider.BeginTransaction();

            await next();
        }
    }
}
