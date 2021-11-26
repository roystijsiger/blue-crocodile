using Microsoft.AspNetCore.Mvc.Rendering;
using PinkPanther.BlueCrocodile.Core.Models;
using PinkPanther.BlueCrocodile.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.POS.ViewModels
{
    public class OrderViewModel
    {
        private IOrderRepository _orderRepositorty;

        public OrderViewModel()
        {
        }

        public OrderViewModel(Order order, IOrderRepository orderRepository)
        {
            Id = order.Id;
            Email = order.Email;
            Code = order.Code;
            Movie = order.Movie;
            ShowTime = order.ShowTime;
            Reservations = order.Reservations;
            Arrangements = order.Arrangements;
            OrderDateTime = order.OrderDateTime;
            PaymentMethod = order.PaymentMethod;
            PaymentStatus = order.PaymentStatus;
            _orderRepositorty = orderRepository;
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }

        public Movie Movie { get; set; }
        public ShowTime ShowTime { get; set; }

        public int? AmountOfAdults { get; set; } = 1;
        public int? AmountOfChildren { get; set; } = 0;

        public List<Reservation> Reservations { get; set; }
        public List<Arrangement> Arrangements { get; set; }

        public IEnumerable<Discount> DiscountsAvailable { get; set; }
        public IEnumerable<Arrangement> ArrangementsAvailable { get; set; }

        public decimal TotalAmount => Reservations.Sum(r => r.Price);
        public DateTime OrderDateTime { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }

        public PaymentStatus? PaymentStatus { get; set; }

        public Order ToOrder()
        {
            return new Order
            {
                Id = Id,
                Email = Email,
                Code = Code,
                Movie = Movie,
                ShowTime = ShowTime,
                Reservations = Reservations,
                Arrangements = Arrangements,
                OrderDateTime = OrderDateTime,
                PaymentMethod = PaymentMethod,
                PaymentStatus = PaymentStatus
            };
        }

        public List<Seat> AvailableSeats()
        {
            var availableSeats = new List<Seat>(ShowTime.Room.Seats);
                       
            var allSeats = ShowTime.Room.Seats;
            var ordersAsEnumerable = _orderRepositorty.GetAllAsync().Result;
            var ordersThatMatter = ordersAsEnumerable.Where(x => x.ShowTime?.Id == ShowTime?.Id).ToList();
            var orders = new List<Order>();

            if (ordersThatMatter.Count() >= 0)
            {
                orders = ordersThatMatter.ToList();
            }


            var reservations = new List<Reservation>();
            foreach (var order in orders)
            {
                if(order.Reservations == null) continue;

                foreach (var reservation in order.Reservations)
                {
                    reservations.Add(reservation);
                }
            }

            var reservedSeats = new List<Seat>();
            foreach (var reservation in reservations)
            {
                reservedSeats.Add(reservation.Seat);
            }

            foreach (var reservedSeat in reservedSeats)
            {
                availableSeats.RemoveAll(x => (x.Row == reservedSeat.Row && x.SeatNumber == reservedSeat.SeatNumber));
            }

            return availableSeats;
        }

        public List<SelectListItem> VisualAvailableSeats()
        {
            var seats = AvailableSeats();
            var visualSeats = new List<SelectListItem>();

            foreach (var seat in seats)
            {
                visualSeats.Add(new SelectListItem("Row: " + seat.Row + ", Seat: " + seat.SeatNumber, seat.Row + "-" + seat.SeatNumber));
            }

            return visualSeats;
        }

        public List<SelectListItem> VisualDiscounts()
        {
            var visualListOfDiscounts = new List<SelectListItem>();

            if (DiscountsAvailable == null)
            {
                return visualListOfDiscounts;
            }

            foreach (var discount in DiscountsAvailable)
            {
                visualListOfDiscounts.Add(new SelectListItem(discount.Name, discount.Name));
            }

            return visualListOfDiscounts;
        }

        public List<SelectListItem> VisualArrangements()
        {
            var visualListOfArrangements = new List<SelectListItem>();

            if (ArrangementsAvailable == null)
            {
                return visualListOfArrangements;
            }

            foreach (var arrangement in ArrangementsAvailable)
            {
                visualListOfArrangements.Add(new SelectListItem(arrangement.Name, arrangement.Name));
            }

            return visualListOfArrangements;
        }
    }
}
