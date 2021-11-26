using System.Collections.Generic;

namespace PinkPanther.BlueCrocodile.Core.Models
{
    public class Movie
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IEnumerable<ShowTime> ShowTimes { get; set; }
    }
}