using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Kyle.Infrastructure.Events;

namespace Kyle.EntityFrameworkExtensions
{
    public class MallDbContext : DbContext
    {
        private readonly IEventBus _eventBus;
        // private readonly IMediator _mediator;
        // private readonly ILogger _logger;
        public MallDbContext(DbContextOptions<MallDbContext> dbContext, IEventBus eventBus
            // , IMediator mediator
            // ,ILogger<MallDbContext> logger
            ) : base(dbContext)
        {
            _eventBus = eventBus;
            // _mediator = mediator;
            // _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MappingEntityTypes(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void MappingEntityTypes(ModelBuilder modelBuilder)
        {
            var assemblies = Kyle.Extensions.AssemblyExtensions.GetAssemblies();

            var types = assemblies.SelectMany(x =>
                x.GetTypes().Where(c => c.GetCustomAttributes<TableAttribute>().Any()).ToArray());
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
                await TriggerDomainEvents(item);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task TriggerDomainEvents(AggregateRoot aggregateRoot)
        {
            var domainEvents = aggregateRoot.DomainEvents.ToList();
            aggregateRoot.DomainEvents.Clear();

            foreach (var domainEvent in domainEvents)
            {
                if (!domainEvent.IsPublisher)
                {
                    // _logger.LogInformation($"Event Send:{domainEvent}");
                    // await _mediator.Publish(domainEvent);
                    await _eventBus.Send(domainEvent);
                }
                // else if (domainEvent.IsPublisher)
                //     await _capPublisher.PublishAsync("Q-Test",domainEvent);
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