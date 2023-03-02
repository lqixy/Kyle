using Kyle.AuthServer;
using Kyle.Infrastructure.ConsulFramework; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryApiScopes(builder.Configuration.GetSection("IdentityServer:ApiScopes"))
    .AddInMemoryApiResources(builder.Configuration.GetSection("IdentityServer:ApiResources"))
    .AddInMemoryClients(builder.Configuration.GetSection("IdentityServer:Clients"))
    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
    .AddProfileService<ProfileService>()
    ;
 
var app = builder.Build();

app.UseIdentityServer();
//app.MapGet("/", () => "Hello World!");
app.MapGet("/health/check", () => Results.Ok());

app.UseConsul(builder.Configuration);

app.Run();
