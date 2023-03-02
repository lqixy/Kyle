using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.LoggerSerilog
{
    public static class SerilogLoggerExtensions
    {
        public static void AddSerilogLogger(this WebApplicationBuilder builder)
        {
            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(builder.Configuration)
            //    .CreateLogger()
            //    ;

            builder.Host.UseSerilog((hostContext, services, configuration) =>
            {
                configuration.ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                //.WriteTo.Console()
                ;
            });


        }

        //public static void UserSerlogLogger(this IApplicationBuilder app)
        //{
        //    app.UseSerilo
        //}
    }
}
