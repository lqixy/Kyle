using Kyle.Extensions.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kyle.Mall.Filters
{
    public class CustomResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor)) return;
            //var originResult = context.Result;

            var ajaxResponse = new AjaxResponse();

            switch (context)
            {
                case ResultExecutingContext resultExecutingContext when resultExecutingContext.Result is ObjectResult:
                    ajaxResponse.Data = ((ObjectResult)resultExecutingContext.Result).Value ?? new { };
                    break;
                case ResultExecutingContext resultExecutingContext when resultExecutingContext.Result is JsonResult:
                    ajaxResponse.Data = ((JsonResult)resultExecutingContext.Result).Value ?? new { };
                    break;
                default:
                    ajaxResponse.Data = new { };
                    break;
            }


            context.Result = new JsonResult(ajaxResponse);
        }
    }
}
