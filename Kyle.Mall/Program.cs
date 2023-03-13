using Kyle.DependencyAutofac;
using Kyle.DependencyServiceCollection;
using Kyle.Infrastructure.ConsulFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Kyle.DapperFrameworkExtensions;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Kyle.LoggerSerilog;
using Kyle.Mall.Middlewares;
using Kyle.Infrastructure.RedisExtensions;
using Kyle.Mall;
using Kyle.Mall.Extensions;
using Kyle.Mall.Filters;
using Kyle.EntityFrameworkExtensions;
using Kyle.Infrastructure.CAP;
using Kyle.Infrastructure.Events;
using Kyle.Infrastructure.Mediators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
//builder.Services.AddTransient<CustomApiFilterMiddleware>();

builder.Services.AddControllers(options =>
{
    //options.Filters.Add(typeof(CustomExceptionFilter));
    options.Filters.Add(typeof(CustomResultFilter));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddLogging();
// builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
builder.Services.AddCAPService(builder.Configuration);

builder.Services.AddServices();
builder.Services.AddEvents();
builder.AddSerilogLogger();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddIdentityServerAuthentication(options =>
    {
        options.Authority = builder.Configuration["AuthServer:Authority"];
        options.ApiName = builder.Configuration["AuthServer:ApiName"];
        options.ApiSecret = builder.Configuration["AuthServer:ApiSecret"];
        options.RequireHttpsMetadata = false;
        options.SupportedTokens = IdentityServer4.AccessTokenValidation.SupportedTokens.Both;
    })
    ;

//builder.Services.AddScrutor();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(x =>
    {
        x.AddMediator();
        x.AddEvents();
        builder.Services.AddAutofac(x);
    })
    ;

//builder.Services.AddDapper();
builder.Services.AddEfCore(builder.Configuration);


builder.Services.AddRedisService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/health/check", () => Results.Ok());

//app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/test"), appBuilde =>
//{
//    appBuilde.UseMiddleware<CustomApiFilterMiddleware>();
//});

//app.UseConsul(builder.Configuration);
//app.UseMiddleware<CustomApiFilterMiddleware>();

//app.UseExceptionHandling();
//app.UseResponseWrapper();

app.Run();
