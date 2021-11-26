using System.Collections.Generic;

namespace PinkPanther.BlueCrocodile.Core.Models
{
    /// <summary>
    ///     A physical cinema room. Contains multiple seats.
    /// </summary>
    public class Room
    {
        public int Number { get; set; }
        public bool WeelChairAccessable { get; set; }
        public List<Seat> Seats { get; set; }
    }
}