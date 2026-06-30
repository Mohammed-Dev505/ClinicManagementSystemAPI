


using ClinicManagement.API.Application.Exceptions;
using ClinicManagement.API.Application.Models;

namespace ClinicManagement.API.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger) => _logger = logger;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context , Exception ex)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorResponseModel
            {
                Message = ex.Message
            };
            switch(ex)
            {
                case NotFoundException:
                    context.Response.StatusCode = 404;
                    response.StatusCode = 404;
                    break;
                case BadRequestException:
                    context.Response.StatusCode = 400;
                    response.StatusCode = 400;
                    break;
                case UnauthorizedException:
                    context.Response.StatusCode = 401;
                    response.StatusCode = 401;
                    break;
                default:
                    context.Response.StatusCode = 500;
                    response.StatusCode = 500;
                    response.Message = "Internal Server Error";
                    break;
            }
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
