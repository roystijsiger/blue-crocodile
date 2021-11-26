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
    public class MovieRepository : IMovieRepository
    {
        private readonly IDataContext _context;

        public MovieRepository(IDataContext context)
        {
            _context = context;
        }

        public async Task<Movie> InsertAsync(Movie movie)
        {
            var dbo = new MovieDbo(movie);
            await _context.Movies.InsertOneAsync(dbo);
            return dbo.ToMovie();
        }

        public async Task InsertManyAsync(List<Movie> orders)
        {
            var dbos = orders.Select(m => new MovieDbo(m));
            await _context.Movies.InsertManyAsync(dbos);
        }

        public async Task<Movie> UpdateAsync(Movie item)
        {
            var dbo = new MovieDbo(item);
            var filter = Builders<MovieDbo>.Filter.Eq(s => s.ObjectId, dbo.ObjectId);
            var result = await _context.Movies.ReplaceOneAsync(filter, dbo);
            if(!result.IsAcknowledged) throw new Exception("Update not acknowledged");

            return await GetAsync(item.Id);
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            var dboList = await _context.Movies.Find(_ => true).ToListAsync();

            return dboList.Select(dbo => dbo.ToMovie());
        }

        public async Task<Movie> GetAsync(string id)
        {
            var dbo = await _context.Movies.Find(m => m.ObjectId == ObjectId.Parse(id)).FirstOrDefaultAsync();

            return dbo?.ToMovie();
        }

        public async Task RemoveAllAsync()
        {
            await _context.Movies.DeleteManyAsync(_ => true);
        }

        public async Task RemoveAsync(string id)
        {
            await _context.Movies.FindOneAndDeleteAsync(m => m.ObjectId == ObjectId.Parse(id));
        }
    }
}
