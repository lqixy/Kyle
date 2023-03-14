using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityServer4.Validation;
using Kyle.AuthServer;
using Kyle.DependencyAutofac;
using Kyle.Encrypt.Application;
using Kyle.Encrypt.Application.Constructs;
using Kyle.EntityFrameworkExtensions;
using Kyle.Infrastructure.Events;
using Kyle.Infrastructure.Mediators;
using Kyle.Members.Application;
using Kyle.Members.Application.Constructs;
using Kyle.Members.Domain;
using Kyle.Members.EntityFramework;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//
// builder.Services.AddDbContext<MallDbContext>(options =>
// {
//     options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
// });
// builder.Services.AddSingleton<MallDbContext>();
// builder.Services.AddMediator() ;

builder.Services.AddSingleton<IIdentityUserPasswordValidatorFactory, IdentityUserPasswordValidatorFactory>()
    .AddTransient<MallUserPasswordValidator>()
    .AddTransient<BackendUserPasswordValidator>()
    // .AddSingleton<IEventBus,EventBus>() 
    // .AddSingleton<IResourceOwnerPasswordValidator,ResourceOwnerPasswordValidator>()
    ;


builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryApiScopes(builder.Configuration.GetSection("IdentityServer:ApiScopes"))
    .AddInMemoryApiResources(builder.Configuration.GetSection("IdentityServer:ApiResources"))
    .AddInMemoryClients(builder.Configuration.GetSection("IdentityServer:Clients"))
    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
    .AddProfileService<ProfileService>()
    ;
 


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(b =>
    {  
        b.AddEvents();
        b.AddMediator();
        b.Register(c =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<MallDbContext>();
            optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            return optionsBuilder.Options;
        });
        
        b.RegisterType<MallDbContext>().AsSelf()
            .InstancePerLifetimeScope();
        b.AddAutofac();
    })
    ;

var app = builder.Build();

app.UseIdentityServer();
//app.MapGet("/", () => "Hello World!");
app.MapGet("/health/check", () => Results.Ok());

// app.UseConsul(builder.Configuration);

app.Run();