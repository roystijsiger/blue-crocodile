using System;

namespace PinkPanther.BlueCrocodile.Core.Models
{
    /// <summary>
    ///     A showtime is one moment a movie will play. It also contains in which cimema and what room.
    /// </summary>
    public class ShowTime
    {
        // public Cinema Cinema { get; set; }
        // public Room Room { get; set; }

        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public Room Room { get; set; }
    }
}