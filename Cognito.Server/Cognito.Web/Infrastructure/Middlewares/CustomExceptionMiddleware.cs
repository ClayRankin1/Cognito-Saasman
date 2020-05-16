using Cognito.Business.Exceptions;
using Cognito.Business.Services.Abstract;
using Cognito.DataAccess.Services.Abstract;
using Cognito.Shared.Exceptions;
using Cognito.Web.Infrastructure.Exceptions;
using Cognito.Web.ViewModels.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Cognito.Web.Infrastructure.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context, ITransactionProvider transactionProvider, IJsonService jsonService)
        {
            var response = new ErrorWrapperViewModel();
            var ranSuccessfully = false;
            var statusCode = HttpStatusCode.InternalServerError;

            try
            {
                await _next.Invoke(context);
                ranSuccessfully = true;
                await transactionProvider.CommitAsync();
            }
            catch (CognitoException ex)
            {
                // TODO: Refactor switch statement to be more extendable
                switch (ex)
                {
                    case InvalidLoginException e:
                        statusCode = HttpStatusCode.Unauthorized;
                        AddExceptioMessageIntoResponse(response, e);
                        break;
                    case TokenRefreshmentFailedException e:
                        statusCode = HttpStatusCode.Forbidden;
                        AddExceptioMessageIntoResponse(response, e);
                        break;
                    case ForbiddenException e:
                        statusCode = HttpStatusCode.Forbidden;
                        break;
                    case EntityNotFoundException e:
                        statusCode = HttpStatusCode.NotFound;
                        AddExceptioMessageIntoResponse(response, e);
                        break;
                    case ClientInvalidOperationException e:
                        statusCode = HttpStatusCode.BadRequest;
                        response.Errors.Add(new ErrorViewModel(e.Title, e.Message));
                        break;
                    default:
                        throw new InvalidOperationException("This type of CognitoException is not handled yet");
                }
            }

            if (!ranSuccessfully)
            {
                // Rollback the transaction if there is some transaction
                await transactionProvider.RollbackAsync();

                if (context.Response.HasStarted)
                {
                    _logger.LogError($"The response has already started, the {nameof(CustomExceptionMiddleware)} will not be executed.");
                    throw new InvalidOperationException($"Can't execute {nameof(CustomExceptionMiddleware)}, the response is in progress.");
                }

                context.Response.Clear();
                context.Response.StatusCode = (int)statusCode;

                await context.Response.WriteAsync(jsonService.SerializeObject(response));

                return;
            }
        }

        private void AddExceptioMessageIntoResponse(ErrorWrapperViewModel response, CognitoException ex)
        {
            if (!string.IsNullOrEmpty(ex.Message))
            {
                response.Errors.Add(new ErrorViewModel(null, ex.Message));
            }
        }
    }
}
