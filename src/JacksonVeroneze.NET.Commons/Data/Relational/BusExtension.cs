using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JacksonVeroneze.NET.Commons.Bus;
using JacksonVeroneze.NET.Commons.DomainObjects;
using JacksonVeroneze.NET.Commons.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JacksonVeroneze.NET.Commons.Data.Relational
{
    public static class MediatorExtension
    {
        public static async Task PublishEvents(this IBus bus, DbContext dbContext)
        {
            IList<EntityEntry<EntityAggregateRoot>> domainEntities = dbContext.ChangeTracker
                .Entries<EntityAggregateRoot>()
                .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any())
                .ToList();

            if (domainEntities.Any() is false) return;

            IList<Event> domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            IEnumerable<Task> tasks = domainEvents
                .Select(async domainEvent => { await bus.PublishEvent(domainEvent); });

            await Task.WhenAll(tasks);
        }
    }
}