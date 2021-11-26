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
    public class ArrangementRepository : IArrangementRepository
    {
        private readonly IDataContext _context;

        public ArrangementRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<Arrangement> InsertAsync(Arrangement arrangement)
        {
            var dbo = new ArrangementDbo(arrangement);
            await _context.Arrangements.InsertOneAsync(dbo);
            return dbo.ToArrangement();
        }

        public async Task InsertManyAsync(List<Arrangement> arrangements)
        {
            var dbos = arrangements.Select(m => new ArrangementDbo(m));
            await _context.Arrangements.InsertManyAsync(dbos);
        }

        public Task<Arrangement> UpdateAsync(Arrangement item) => throw new NotImplementedException();

        public async Task<IEnumerable<Arrangement>> GetAllAsync()
        {
            var dboList = await _context.Arrangements.Find(_ => true).ToListAsync();
            
            return dboList.Select(dbo => dbo.ToArrangement());
        }

        public async Task<Arrangement> GetAsync(string id)
        {
            var dbo = await _context.Arrangements.Find(m => m.ObjectId == ObjectId.Parse(id)).FirstOrDefaultAsync();

            return dbo?.ToArrangement();
        }

        public async Task RemoveAllAsync()
        {
             await _context.Arrangements.DeleteManyAsync(_ => true);
        }

        public async Task RemoveAsync(string id)
        {
             await _context.Arrangements.FindOneAndDeleteAsync(m => m.ObjectId == ObjectId.Parse(id));
        }
    }
}
