using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PinkPanther.BlueCrocodile.Core.Models;

namespace PinkPanther.BlueCrocodile.Infrastructure.Data.Models
{
    public class MoviePassDbo
    {
        public MoviePassDbo(MoviePass moviePass)
        {
            if(!ObjectId.TryParse(moviePass.Id, out ObjectId objectId))
            {
                ObjectId = ObjectId.GenerateNewId();
            }

            ObjectId = objectId;
            Price = moviePass.Price;
        }

        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ObjectId { get; set; }
        public decimal Price { get; set; }

        public MoviePass ToMoviePass()
        {
            return new MoviePass
            {
                Id = ObjectId.ToString(),
                Price = Price
            };
        }

    }
}
