var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
    .AddInMemoryApiScopes(builder.Configuration.GetSection("IdentityServer:ApiScopes"))
    .AddInMemoryApiResources(builder.Configuration.GetSection("IdentityServer:ApiResources"))
    .AddInMemoryClients(builder.Configuration.GetSection("IdentityServer:Clients"))

    ;

var app = builder.Build();

app.UseIdentityServer();
//app.MapGet("/", () => "Hello World!");

app.Run();
