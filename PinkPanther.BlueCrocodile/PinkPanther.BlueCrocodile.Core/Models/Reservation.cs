namespace PinkPanther.BlueCrocodile.Core.Models
{
    /// <summary>
    ///     A reservation is a ticket for a specific showtime.
    /// </summary>
    public class Reservation
    {
        public string Id { get; set; }
        public string Code { get; set; }

        public Seat Seat { get; set; }

        public decimal Price { get; set; }
        public Discount Discount { get; set; }
        public Arrangement Arrangement { get; set; }
    }
}