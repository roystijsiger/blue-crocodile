using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PinkPanther.BlueCrocodile.Core.Models;

namespace PinkPanther.BlueCrocodile.Infrastructure.Data.Models
{
    public class SubscriptionDbo
    {
        public SubscriptionDbo(Subscription moviePass)
        {
            if(!ObjectId.TryParse(moviePass.Id, out ObjectId objectId))
            {
                objectId = ObjectId.GenerateNewId();
            }

            ObjectId = objectId;

            Price = moviePass.Price;
            TimeSpan = TimeSpan;
        }

        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ObjectId { get; set; }
        public decimal Price { get; set; }
        public string TimeSpan { get; set; }

        public Subscription ToSubscription()
        {
            return new Subscription
            {
                Id = ObjectId.ToString(),
                Price = Price,
                TimeSpan = TimeSpan
            };
        }
    }
}
