using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers
{
    public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
    {
        private readonly IEventStore _eventStore;
        private readonly IEventProducer _eventProducer;

        public EventSourcingHandler(IEventStore eventStore, IEventProducer eventProducer)
        {
            _eventStore = eventStore;
            _eventProducer = eventProducer;
        }
        public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
        {
            var aggregate = new PostAggregate();
            var events = await _eventStore.GetEventsAsync(aggregateId);
            if(events == null || !events.Any()) return aggregate;
            aggregate.ReplayEvents(events);
            aggregate.Version = events.Select(x=>x.Version).Max();
            return aggregate;
        }

        public async Task RepublishEventsAsync()
        {
            var aggredateIds = await _eventStore.GetAggregateIdAsync();
            if(aggredateIds == null || !aggredateIds.Any()) return;
            foreach (var aggredateId in aggredateIds)
            {
                var aggredate = await GetByIdAsync(aggredateId);
                if(aggredate == null || !aggredate.Active) continue;
                var events = await _eventStore.GetEventsAsync(aggredateId);
                foreach (var @event in events)
                {
                    var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                    await _eventProducer.ProduceAsync(topic!, @event);
                }
            }
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id,aggregate.GetUnCommittedChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }
    }
}