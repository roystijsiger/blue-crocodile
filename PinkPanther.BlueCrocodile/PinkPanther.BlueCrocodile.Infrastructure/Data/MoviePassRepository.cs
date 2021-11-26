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
    public class MoviePassRepository : IMoviePassRepository
    {
        private readonly IDataContext _context;

        public MoviePassRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<MoviePass> InsertAsync(MoviePass item)
        {
            var dbo = new MoviePassDbo(item);
            await _context.MoviePasses.InsertOneAsync(dbo);
            return dbo.ToMoviePass();
        }

        public async Task InsertManyAsync(List<MoviePass> items)
        {
            var dbos = items.Select(m => new MoviePassDbo(m));
            await _context.MoviePasses.InsertManyAsync(dbos);
        }

        public Task<MoviePass> UpdateAsync(MoviePass item) => throw new NotImplementedException();

        public async Task<IEnumerable<MoviePass>> GetAllAsync()
        {
            var dboList = await _context.MoviePasses.Find(_ => true).ToListAsync();
            
            return dboList.Select(dbo => dbo.ToMoviePass());
        }

        public async Task<MoviePass> GetAsync(string id)
        {
            var dbo = await _context.MoviePasses.Find(m => m.ObjectId == ObjectId.Parse(id)).FirstOrDefaultAsync();

            return dbo?.ToMoviePass();
        }

        public async Task RemoveAllAsync()
        {
             await _context.MoviePasses.DeleteManyAsync(_ => true);
        }

        public async Task RemoveAsync(string id)
        {
             await _context.MoviePasses.FindOneAndDeleteAsync(m => m.ObjectId == ObjectId.Parse(id));
        }
    }
}
