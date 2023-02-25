using Kyle.DependencyScrutor;
using Kyle.Infrastructure.ConsulFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Kyle.DapperFrameworkExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddScrutor();

builder.Services.AddDapper();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/health/check", () => Results.Ok());

app.UseConsul(builder.Configuration);

app.Run();
