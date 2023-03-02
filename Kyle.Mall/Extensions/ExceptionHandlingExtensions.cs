using Kyle.Mall.Middlewares;

namespace Kyle.Mall.Extensions
{
    public static class ExceptionHandlingExtensions
    {
        public static void UseExceptionHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
