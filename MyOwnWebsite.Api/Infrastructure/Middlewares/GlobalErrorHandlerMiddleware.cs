using System.Net;
using System.Text.Json;
using MyOwnWebsite.Application.Exceptions;
using MyOwnWebsite.Application.Wrappers;

namespace MyOwnWebsite.Api.Infrastructure.Middlewares;



public class GlobalErrorHandlerMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exeption)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new BaseResult<string>(new Error(ErrorCode.Exception, exeption?.Message));


			//here add ur custom exceptions to catch and map them to baseResponse
			switch (exeption)
            {
                case ApplicationNotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    responseModel.Errors = [new Error(ErrorCode.NotFound, ex.Message, ex.FieldName ?? string.Empty)];
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var result = JsonSerializer.Serialize(responseModel);

            await response.WriteAsync(result);
        }
    }
}

public static class GlobalErrorHandlerMiddlewareExtension
{
    public static IApplicationBuilder UseGlobalErrorHandlerMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalErrorHandlerMiddleware>();
    }
}

