using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PinkPanther.BlueCrocodile.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace PinkPanther.BlueCrocodile.WebApi.Data
{
    public class MovieDbo
    {
        public MovieDbo(Movie movie)
        {
            if(!ObjectId.TryParse(movie.Id, out ObjectId objectId))
            {
                objectId = ObjectId.GenerateNewId();
            }

            ObjectId = objectId;
            Title = movie.Title;
            Description = movie.Description;
            Image = movie.Image;
            ShowTimes = movie.ShowTimes.Select(s => new ShowTimeDbo(s));
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ObjectId { get; set; }
        
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IEnumerable<ShowTimeDbo> ShowTimes { get; set; }

        public Movie ToMovie()
        {
            return new Movie {
                Id = ObjectId.ToString(),
                Title = Title,
                Description = Description,
                Image = Image,
                ShowTimes = ShowTimes.Select(s => s.ToShowTime())
            };
        }
    }
}
