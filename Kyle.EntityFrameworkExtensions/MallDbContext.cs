using Kyle.Extensions;
using Kyle.Infrastructure;
using Kyle.Infrastructure.Commanding;
using Kyle.Infrastructure.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json.Linq;
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

        //private readonly ICommandSender commandSender;
        public IEventBus EventBus { get; set; } = new EventBus();
        private readonly IEventBus eventBus;

        public MallDbContext(DbContextOptions<MallDbContext> dbContext) : base(dbContext)
        {
            this.eventBus = eventBus;
            //this.commandSender = commandSender;
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
            var list = types?.Where(x => x.IsClass && !x.IsAbstract && !x.IsGenericType).ToList();

            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    modelBuilder.Entity(item);
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is AggregateRoot && ((AggregateRoot)x.Entity).DomainEvents.Any())
                .Select(x => (AggregateRoot)x.Entity).ToList();

            foreach (var item in entities)
            {
                //var messageData = new MessageData { CommandName = item.GetType().FullName, MessageDataBody = JToken.FromObject(item) };
                //commandSender.PublishMessage(messageData);
                TriggerDomainEvents(item);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void TriggerDomainEvents(AggregateRoot aggregateRoot)
        {
            var domainEvents = aggregateRoot.DomainEvents.ToList();
            aggregateRoot.DomainEvents.Clear();

            foreach (var domainEvent in domainEvents.Where(x => !x.IsPublisher))
            {
                eventBus.Trigger(domainEvent.GetType(), domainEvent);
            }
        }

        //public override int SaveChanges()
        //{
        //    ChangeTracker.Entries()
        //        .Where(x=>x.Entity is AggregateRoot)
        //        ;
        //    return base.SaveChanges();
        //}

    }
}
