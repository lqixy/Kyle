using Kyle.Encrypt.Application;
using Kyle.Encrypt.Application.Constructs;
using Kyle.EntityFrameworkExtensions;
using Kyle.Members.Application.Constructs;
using Kyle.Members.Domain;
using Kyle.Members.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Members.Application.Test
{
    //public class TestBase
    //{
    //    protected readonly IServiceProvider Provider;

    //    public TestBase()
    //    {
    //        var configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json")
    //            .Build()
    //            ;

    //        var services = new ServiceCollection();
    //        services.AddDbContext<MallDbContext>(options =>
    //        {
    //            options.UseSqlServer(configuration["ConnectionStrings:Default"]);
    //        });

    //        services.AddSingleton<IUserRegisterAppService, UserRegisterAppService>();
    //        services.AddSingleton<IUserOperationRepository, UserOperationRepository>();
    //        services.AddSingleton<IUserQueryRepository, UserQueryRepository>();
    //        services.AddSingleton<IUserRegisterRecordRepository, UserRegisterRecordRepository>();
    //        services.AddSingleton<IEncryptAppService, EncryptAppService>();

    //        Provider = services.BuildServiceProvider();
    //    }
    //}
}
