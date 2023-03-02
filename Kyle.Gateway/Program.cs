using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

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


builder.Services.AddOcelot()
    .AddConsul()
    ;

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

app.UseOcelot();

app.Run();
