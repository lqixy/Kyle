using Microsoft.AspNetCore.Http.Extensions;

namespace Kyle.Mall.Middlewares
{
    public class CustomApiFilterMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        public CustomApiFilterMiddleware(ILogger<CustomApiFilterMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var request = context.Request;
            // 启用读取request
            context.Request.EnableBuffering();

            using var requestReader = new StreamReader(request.Body);
            var requestContent = await requestReader.ReadToEndAsync();
            request.Body.Position = 0;

            // 设置stream存放ResponseBody
            var responseOriginalBody = context.Response.Body;
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            _logger.LogInformation($"访问记录 URI:{request.GetDisplayUrl()} IP:{context.Connection.RemoteIpAddress} Referrer:{request.Headers["Referer"]} UserAgent:{request.Headers["User-Agent"]} Body:{requestContent} ");

            await next(context);

            memoryStream.Position = 0;
            var responseReader = new StreamReader(memoryStream);
            var responseContent = await responseReader.ReadToEndAsync();
            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(responseOriginalBody);
            context.Response.Body = responseOriginalBody;

            _logger.LogInformation($"返回记录 {context.Response.StatusCode} {responseContent}");


        }
    }
}
