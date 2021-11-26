using MongoDB.Bson;
using MongoDB.Driver;
using PinkPanther.BlueCrocodile.Core.Models;
using PinkPanther.BlueCrocodile.Core.Repositories;
using PinkPanther.BlueCrocodile.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.Infrastructure.Data
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly IDataContext _context;

        public SubscriptionRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<Subscription> InsertAsync(Subscription item)
        {
            var dbo = new SubscriptionDbo(item);
            await _context.Subscriptions.InsertOneAsync(dbo);
            return dbo.ToSubscription();
        }

        public async Task InsertManyAsync(List<Subscription> items)
        {
            var dbos = items.Select(m => new SubscriptionDbo(m));
            await _context.Subscriptions.InsertManyAsync(dbos);
        }

        public Task<Subscription> UpdateAsync(Subscription item) => throw new NotImplementedException();

        public async Task<IEnumerable<Subscription>> GetAllAsync()
        {
            var dboList = await _context.Subscriptions.Find(_ => true).ToListAsync();
            
            return dboList.Select(dbo => dbo.ToSubscription());
        }

        public async Task<Subscription> GetAsync(string id)
        {
            var dbo = await _context.Subscriptions.Find(m => m.ObjectId == ObjectId.Parse(id)).FirstOrDefaultAsync();

            return dbo?.ToSubscription();
        }

        public async Task RemoveAllAsync()
        {
             await _context.Subscriptions.DeleteManyAsync(_ => true);
        }

        public async Task RemoveAsync(string id)
        {
             await _context.Subscriptions.FindOneAndDeleteAsync(m => m.ObjectId == ObjectId.Parse(id));
        }
    }
}
