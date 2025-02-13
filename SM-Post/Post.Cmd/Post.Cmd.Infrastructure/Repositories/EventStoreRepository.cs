using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;
using CQRS.Core.Infrastructure;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Cmd.Infrastructure.Config;

namespace Post.Cmd.Infrastructure.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventStoreCollection;
        public EventStoreRepository(IOptions<MongoDbConfig> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(config.Value.DataBase);
            _eventStoreCollection = mongoDataBase.GetCollection<EventModel>(config.Value.Collection);
        }

        public async Task<List<EventModel>> FindAllAsync()
        {
            return await _eventStoreCollection
                .Find( _ => true)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        {
            return await _eventStoreCollection
                .Find(x => x.AggregateIdentifier == aggregateId )
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task SaveAsync(EventModel @event)
        {
            await _eventStoreCollection
                .InsertOneAsync(@event)
                .ConfigureAwait(false);
        }
    }
}