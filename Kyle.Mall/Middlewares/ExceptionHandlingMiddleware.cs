using Kyle.Extensions;
using Kyle.Extensions.Exceptions;
using Kyle.Extensions.WebApi.Models;
using Newtonsoft.Json;

namespace Kyle.Mall.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errResponse = new AjaxResponse() { Success = false };

            switch (exception)
            {
                case KyleException ex:
                    errResponse.SetMessage(ex.Message);
                    break;
                default:
                    errResponse.SetMessage("出错了!");
                    break;
            }

            _logger.LogError(exception.Message);
            var result = JsonConvert.SerializeObject(errResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
