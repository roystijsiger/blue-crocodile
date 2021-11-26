namespace PinkPanther.BlueCrocodile.Core.Models
{
    /// <summary>
    ///     A physical seat contains a row and seat number.
    /// </summary>
    public class Seat
    {
        public string Location { get; set; }
        public int Row { get; set; }
        public int SeatNumber { get; set; }
    }
}