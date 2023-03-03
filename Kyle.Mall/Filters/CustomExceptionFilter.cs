using Kyle.Extensions.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kyle.Mall.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor)) return;

            _logger.LogError(context.Exception, context.Exception.Message);

            context.Result = new ObjectResult(new AjaxResponse(1103, "出错了!请稍后再试."));
        }
    }
}
