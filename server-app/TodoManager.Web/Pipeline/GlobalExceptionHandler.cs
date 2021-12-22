using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using TodoManager.BusinessExceptions;

namespace TodoManager.Web.Pipeline
{
    public static class GlobalExceptionHandler
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                    switch (ex)
                    {
                        case BadHttpRequestException exception:
                            context.Response.StatusCode = exception.StatusCode;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsJsonAsync(new ErrorDetails
                            {
                                Message = exception.Message
                            });
                            break;
                        case BusinessException exception:
                            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsJsonAsync(new BusinessExceptionErrorDetails()
                            {
                                ErrorCode = exception.ErrorCode,
                                Message = ex.Message
                            });
                            break;
                    }
                });
            });
        }
    }
}