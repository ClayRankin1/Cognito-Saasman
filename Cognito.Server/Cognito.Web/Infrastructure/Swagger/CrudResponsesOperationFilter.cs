using Cognito.DataAccess.Repositories.Results;
using Cognito.Shared.Extensions;
using Cognito.Web.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cognito.Web.Infrastructure.Swagger
{
    public class CrudResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor controllerDescriptor)
            {
                var baseType = controllerDescriptor.ControllerTypeInfo.BaseType?.GetTypeInfo();

                // Get type and see if its a generic controller with a single type parameter
                if (baseType == null || !baseType.IsGenericType)
                {
                    return;
                }

                var genericTypeDefinition = baseType.GetGenericTypeDefinition();
                if (genericTypeDefinition != typeof(CrudControllerBase<,,,,>) && genericTypeDefinition != typeof(CrudControllerBase<,,,>))
                {
                    return;
                }

                var entityType = baseType.GenericTypeArguments[0];
                var viewModelType = baseType.GenericTypeArguments.Length == 5 
                    ? baseType.GenericTypeArguments[3]
                    : baseType.GenericTypeArguments[2];

                // TODO: FIXME JIRI - Think about this clearing when some controller override base actions
                operation.Responses.Clear();

                if (context.ApiDescription.HttpMethod == "GET" && context.MethodInfo.Name == "Get")
                {
                    operation.Summary = $"Gets the specific {entityType.Name} by it's Id.";
                    operation
                        .AddResponse(context, ResponseKeys.Success, viewModelType)
                        .AddResponse(context, ResponseKeys.Unauthorized)
                        .AddResponse(context, ResponseKeys.NotFound)
                        .AddResponse(context, ResponseKeys.ServerError);
                }

                if (context.ApiDescription.HttpMethod == "GET" && context.MethodInfo.Name == "GetAll")
                {
                    operation.Summary = $"Gets all {entityType.Name.Pluralize()}.";
                    operation
                        .AddResponse(context, ResponseKeys.Success, viewModelType.MakeArrayType())
                        .AddResponse(context, ResponseKeys.Unauthorized)
                        .AddResponse(context, ResponseKeys.ServerError);
                }

                if (context.ApiDescription.HttpMethod == "POST" && context.MethodInfo.Name == "GetFilteredData")
                {
                    operation.Summary = $"Gets filtered, sorted and paged data.";
                    operation
                        .AddResponse(context, ResponseKeys.Success, typeof(PaginatedList<>).MakeGenericType(viewModelType))
                        .AddResponse(context, ResponseKeys.Unauthorized)
                        .AddResponse(context, ResponseKeys.ServerError);
                }

                if (context.ApiDescription.HttpMethod == "POST" && context.MethodInfo.Name == "Create")
                {
                    operation.Summary = $"Creates new {entityType.Name}.";
                    operation
                        .AddResponse(context, ResponseKeys.Created, viewModelType)
                        .AddResponse(context, ResponseKeys.BadRequest)
                        .AddResponse(context, ResponseKeys.Unauthorized)
                        .AddResponse(context, ResponseKeys.ServerError);
                }

                if (context.ApiDescription.HttpMethod == "PUT" && context.MethodInfo.Name == "Update")
                {
                    operation.Summary = $"Updates the specific {entityType.Name}.";
                    operation
                        .AddResponse(context, ResponseKeys.NoContent)
                        .AddResponse(context, ResponseKeys.BadRequest)
                        .AddResponse(context, ResponseKeys.Unauthorized)
                        .AddResponse(context, ResponseKeys.ServerError);
                }

                if (context.ApiDescription.HttpMethod == "DELETE" && context.MethodInfo.Name == "Delete")
                {
                    operation.Summary = $"Deletes the specific {entityType.Name}.";
                    operation
                        .AddResponse(context, ResponseKeys.NoContent)
                        .AddResponse(context, ResponseKeys.Unauthorized)
                        .AddResponse(context, ResponseKeys.ServerError);
                }
            }
        }
    }

    internal enum ResponseKeys
    {
        Success = 200,

        Created = 201,

        NoContent = 204,

        BadRequest = 400,

        Unauthorized = 401,

        NotFound = 404,

        ServerError = 500
    }

    internal static class OpenApiOperationExtensions
    {
        private static readonly string ApplicationJson = "application/json";

        public static OpenApiOperation AddResponse(
            this OpenApiOperation operation,
            OperationFilterContext context,
            ResponseKeys responseKey,
            Type responseType = null)
        {
            var response = new OpenApiResponse
            {
                Description = responseKey.ToString()
            };

            if (responseType != null)
            {
                response.Content = new Dictionary<string, OpenApiMediaType>()
                {
                    {
                        ApplicationJson, new OpenApiMediaType()
                        {
                            Schema = GetSchema(context, responseType)
                        }
                    }

                };
            }

            operation.Responses.Add(((int)responseKey).ToString(), response);

            return operation;
        }



        private static OpenApiSchema GetSchema(OperationFilterContext context, Type type)
        {
            if (!context.SchemaRepository.Schemas.TryGetValue(type.Name, out var schema))
            {
                schema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);
            }

            return schema;
        }
    }
}
