using MongoDB.Driver;
using PinkPanther.BlueCrocodile.Infrastructure.Data.Models;
using PinkPanther.BlueCrocodile.WebApi.Data;

namespace PinkPanther.BlueCrocodile.Infrastructure.Data
{
    public class DataContext : IDataContext
    {
        public const string DatabaseName = "pinkpanther";
        public readonly MongoClient client;

        public IMongoCollection<MovieDbo> Movies { get; }
        public IMongoCollection<OrderDbo> Orders { get; }
        public IMongoCollection<DiscountDbo> Discounts { get; }
        public IMongoCollection<ArrangementDbo> Arrangements { get; }
        public IMongoCollection<MoviePassDbo> MoviePasses { get; }
        public IMongoCollection<SubscriptionDbo> Subscriptions { get; }

        public DataContext(string connectionString)
        {
            client = new MongoClient(connectionString);
            var db = client.GetDatabase(DatabaseName);

            Movies = db.GetCollection<MovieDbo>("movies");
            Orders = db.GetCollection<OrderDbo>("orders");
            Discounts = db.GetCollection<DiscountDbo>("discounts");
            Arrangements = db.GetCollection<ArrangementDbo>("arrangements");
            MoviePasses = db.GetCollection<MoviePassDbo>("movie_passes");
            Subscriptions = db.GetCollection<SubscriptionDbo>("subscriptions");
        }
    }
}
