using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PinkPanther.BlueCrocodile.Core.Models;

namespace PinkPanther.BlueCrocodile.WebApi.Data
{
    public class ArrangementDbo
    {
        public ArrangementDbo(Arrangement arrangement)
        {
            Name = arrangement.Name;
            Ammount = arrangement.Ammount;
            Description = arrangement.Description;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ObjectId { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Ammount { get; set; }

        public Arrangement ToArrangement()
        {
            return new Arrangement {
               Name = this.Name,
               Description = this.Description,
               Ammount = this.Ammount
            };
        }
    }
}
