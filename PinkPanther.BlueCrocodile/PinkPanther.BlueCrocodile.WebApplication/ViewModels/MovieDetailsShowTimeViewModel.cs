using System;

namespace PinkPanther.BlueCrocodile.WebApplication.ViewModels
{
    public class MovieDetailsShowTimeView
    {
        public string MovieId { get; set; }
        public string ShowTimeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime DateTime { get; set; }
        public int Room { get; set; }

    }
}
