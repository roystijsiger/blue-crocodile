using MongoDB.Bson;
using MongoDB.Driver;
using PinkPanther.BlueCrocodile.Core.Models;
using PinkPanther.BlueCrocodile.Core.Repositories;
using PinkPanther.BlueCrocodile.WebApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.Infrastructure.Data
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IDataContext _context;

        public DiscountRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<Discount> InsertAsync(Discount discount)
        {
            var dbo = new DiscountDbo(discount);
            await _context.Discounts.InsertOneAsync(dbo);
            return dbo.ToDiscount();
        }

        public async Task InsertManyAsync(List<Discount> discounts)
        {
            var dbos = discounts.Select(m => new DiscountDbo(m));
            await _context.Discounts.InsertManyAsync(dbos);
        }

        public Task<Discount> UpdateAsync(Discount item) => throw new NotImplementedException();

        public async Task<IEnumerable<Discount>> GetAllAsync()
        {
            var dboList = await _context.Discounts.Find(_ => true).ToListAsync();
            
            return dboList.Select(dbo => dbo.ToDiscount());
        }

        public async Task<Discount> GetAsync(string id)
        {
            var dbo = await _context.Discounts.Find(m => m.ObjectId == ObjectId.Parse(id)).FirstOrDefaultAsync();

            return dbo?.ToDiscount();
        }

        public async Task RemoveAllAsync()
        {
             await _context.Discounts.DeleteManyAsync(_ => true);
        }

        public async Task RemoveAsync(string id)
        {
             await _context.Discounts.FindOneAndDeleteAsync(m => m.ObjectId == ObjectId.Parse(id));
        }
    }
}
