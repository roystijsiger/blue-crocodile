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
    public class OrderRepository : IOrderRepository
    {
        private readonly IDataContext _context;
        private IMovieRepository _movieRepository;

        public OrderRepository(IDataContext context, IMovieRepository movieRepository)
        {
            _context = context;
            _movieRepository = movieRepository;
        }

        public async Task<Order> InsertAsync(Order order)
        {
            if (order.Movie?.Id == null) throw new ArgumentException("MovieId of order is not set", nameof(order));
            if (order.ShowTime?.Id == null) throw new ArgumentException("ShowTime of order is not set", nameof(order));

            var dbo = new OrderDbo(order);
            await _context.Orders.InsertOneAsync(dbo);

            // Get the order after insert. The referenced movie or showtime could have been changed.
            return await GetAsync(dbo.ObjectId.ToString());
        }

        public async Task InsertManyAsync(List<Order> orders)
        {
            if (orders.Any(o => o.Movie?.Id == null))
                throw new ArgumentException("MovieId of any of the order is not set", nameof(orders));

            if (orders.Any(o => o.ShowTime?.Id == null))
                throw new ArgumentException("ShowTime of any of the orders is not set", nameof(orders));

            var dbos = orders.Select(o => new OrderDbo(o));
            await _context.Orders.InsertManyAsync(dbos);
        }

        public async Task<Order> UpdateAsync(Order item)
        {
            var dbo = new OrderDbo(item);

            var filter = Builders<OrderDbo>.Filter.Eq(p => p.ObjectId, dbo.ObjectId);
            var replaceOneResult = _context.Orders.ReplaceOneAsync(filter, dbo).Result;

            if(!replaceOneResult.IsAcknowledged) throw new Exception("Update query is not acknowledged");

            return await GetAsync(item.Id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var dboList = await _context.Orders.Find(_ => true).ToListAsync();
            var result = new List<Order>();
            foreach (var orderDbo in dboList) {
                var order = orderDbo.ToOrder();
                order.Movie = await _movieRepository.GetAsync(orderDbo.MovieId);
                order.ShowTime = order.Movie?.ShowTimes.FirstOrDefault(s => s.Id == orderDbo.ShowTimeId);
                
                result.Add(order);
            }

            return result;
        }

        public async Task<Order> GetAsync(string id)
        {
            var dbo = await _context.Orders.Find(m => m.ObjectId == ObjectId.Parse(id)).FirstOrDefaultAsync();

            var order = dbo.ToOrder();
            order.Movie = await _movieRepository.GetAsync(dbo.MovieId);
            order.ShowTime = order.Movie?.ShowTimes.FirstOrDefault(s => s.Id == dbo.ShowTimeId);

            return order;
        }

        public async Task RemoveAllAsync()
        {
            await _context.Orders.DeleteManyAsync(_ => true);
        }

        public async Task RemoveAsync(string id)
        {
            await _context.Orders.FindOneAndDeleteAsync(m => m.ObjectId == ObjectId.Parse(id));
        }

        public async Task<Order> GetByCodeAsync(string code)
        {
            var dbo = await _context.Orders.Find(m => m.Code == code).FirstOrDefaultAsync();

            if (dbo == null) return null;

            var order = dbo.ToOrder();
            order.Movie = await _movieRepository.GetAsync(dbo.MovieId);
            order.ShowTime = order.Movie?.ShowTimes.FirstOrDefault(s => s.Id == dbo.ShowTimeId);

            return order;
        }
    }
}
