using MongoDB.Driver;
using PinkPanther.BlueCrocodile.Infrastructure.Data.Models;
using PinkPanther.BlueCrocodile.WebApi.Data;

namespace PinkPanther.BlueCrocodile.Infrastructure.Data
{
    public interface IDataContext
    {
        IMongoCollection<MovieDbo> Movies { get; }
        IMongoCollection<OrderDbo> Orders { get; }
        IMongoCollection<DiscountDbo> Discounts { get; }
        IMongoCollection<ArrangementDbo> Arrangements { get; }
        IMongoCollection<MoviePassDbo> MoviePasses { get; }
        IMongoCollection<SubscriptionDbo> Subscriptions { get; }
    }
}
