using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PinkPanther.BlueCrocodile.Core.Models;
using System;

namespace PinkPanther.BlueCrocodile.WebApi.Data
{
    public class ShowTimeDbo
    {
        public ShowTimeDbo(ShowTime showTime)
        {
            if(!ObjectId.TryParse(showTime.Id, out ObjectId objectId))
            {
                objectId = ObjectId.GenerateNewId();
            }

            ObjectId = objectId;
            DateTime = showTime.DateTime;
            Room = showTime.Room;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ObjectId { get; set; }

        public DateTime DateTime { get; set; }
        public Room Room { get; set; }

        public ShowTime ToShowTime()
        {
            return new ShowTime
            {
                Id = ObjectId.ToString(),
                DateTime = DateTime,
                Room = Room
            };
        }
    }
}
