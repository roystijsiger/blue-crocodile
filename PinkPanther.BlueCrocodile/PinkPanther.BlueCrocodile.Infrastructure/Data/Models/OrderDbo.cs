using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PinkPanther.BlueCrocodile.Core.Models;
using System;
using System.Collections.Generic;

namespace PinkPanther.BlueCrocodile.WebApi.Data
{
    public class OrderDbo
    {
        public OrderDbo(Order order)
        {
            if(!ObjectId.TryParse(order.Id, out ObjectId objectId))
            {
                objectId = ObjectId.GenerateNewId();
            }

            ObjectId = objectId;
            Email = order.Email;
            Code = order.Code;
            MovieId = order.Movie?.Id;
            ShowTimeId = order.ShowTime?.Id;
            Reservations = order.Reservations;
            OrderDateTime = order.OrderDateTime;
            PaymentMethod = order.PaymentMethod;
            PaymentStatus = order.PaymentStatus;
            PaymentId = order.PaymentId;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ObjectId { get; set; }
        public string Email { get; set; }

        public string MovieId { get; set; }
        public string ShowTimeId { get; set; }
        public string Code { get; set; }

        public List<Reservation> Reservations { get; set; }
        public IDictionary<int, string> Arrangements { get; set; }

        public DateTime OrderDateTime { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public string PaymentId { get; set; }

        public Order ToOrder()
        {
            return new Order {
                Id = ObjectId.ToString(),
                Email = Email,
                Code = Code,
                Reservations = Reservations,
                OrderDateTime = OrderDateTime,
                PaymentMethod = PaymentMethod,
                PaymentStatus = PaymentStatus,
                PaymentId = PaymentId
            };
        }
    }
}
