using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PinkPanther.BlueCrocodile.Core.Models;

namespace PinkPanther.BlueCrocodile.WebApi.Data
{
    public class DiscountDbo
    {
        public DiscountDbo(Discount discount)
        {
            Name = discount.Name;
            Ammount = discount.Ammount;
            Description = discount.Description;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ObjectId { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Ammount { get; set; }

        public Discount ToDiscount()
        {
            return new Discount {
               Name = this.Name,
               Description = this.Description,
               Ammount = this.Ammount
            };
        }
    }
}
