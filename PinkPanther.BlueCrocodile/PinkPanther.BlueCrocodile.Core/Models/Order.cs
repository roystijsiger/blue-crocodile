using System;
using System.Collections.Generic;
using System.Linq;

namespace PinkPanther.BlueCrocodile.Core.Models
{
    /// <summary>
    ///     An order can be placed by a customer and contains multiple reservations for multiple people.
    /// </summary>
    public class Order
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }

        public Movie Movie { get; set; }
        public ShowTime ShowTime { get; set; }

        public List<Reservation> Reservations { get; set; }
        public List<Arrangement> Arrangements { get; set; }

        public decimal? TotalAmount => Reservations?.Sum(r => r.Price);
        public DateTime OrderDateTime { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public string PaymentId { get; set; }
    }

    public enum PaymentMethod
    {
        CreditCard,
        IDeal,
        AtCounter
    }

    public enum PaymentStatus
    {
        Open,
        Canceled,
        Pending,
        Paid,
        PaidOut,
        Refunded,
        Expired,
        Failed,
        Charged_Back
    }
}