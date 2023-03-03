using Kyle.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.EntityFrameworkExtensions
{
    public class MallDbContext : DbContext
    {
        public MallDbContext(DbContextOptions<MallDbContext> dbContext) : base(dbContext)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MappingEntityTypes(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void MappingEntityTypes(ModelBuilder modelBuilder)
        {
            var assemblies = Kyle.Extensions.AssemblyExtensions.GetAssemblies();

            var types = assemblies.SelectMany(x => x.GetTypes().Where(c => c.GetCustomAttributes<TableAttribute>().Any()).ToArray());
            //var list = types?.Where(x => x.IsSubclassOf(typeof(BaseModel<>)) || x.IsSubclassOf(typeof(BaseViewModel))).ToList();
            var list = types?.Where(x => x.IsClass && !x.IsAbstract && !x.IsGenericType).ToList();

            //var entityMethod = typeof(ModelBuilder).GetMethod("Entity", new Type[] { });

            if (list != null && list.Any())
            {
                foreach (var item in list)
                { 
                    modelBuilder.Entity(item);
                    //modelBuilder.Model.AddEntityType(item);

                }
            }

        }
    }
}
