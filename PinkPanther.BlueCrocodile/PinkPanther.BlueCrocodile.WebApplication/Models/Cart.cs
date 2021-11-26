using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PinkPanther.BlueCrocodile.Core.Models;
using System;
using System.Collections.Generic;

namespace PinkPanther.BlueCrocodile.WebApplication.Models
{
    public class Cart
    {
        private readonly ISession _session;


        public List<string> Subscriptions { get; set; }
        public List<string> Orders { get; set; }
        public List<string> MoviePasses { get; set; }

        public decimal TotalPrice { get; set; }


        public Cart(ISession session)
        {
            _session = session;

            var ordersJson = _session.GetString("cart_orders");
            Orders = ordersJson != null ? JsonConvert.DeserializeObject<List<string>>(ordersJson) : new List<string>();


            var subscriptionsJson = _session.GetString("cart_subscriptions");
            Subscriptions = subscriptionsJson != null ? JsonConvert.DeserializeObject<List<string>>(subscriptionsJson) : new List<string>();

            var moviePassesJson = _session.GetString("cart_movie_passes");
            MoviePasses = moviePassesJson != null ? JsonConvert.DeserializeObject<List<string>>(moviePassesJson) : new List<string>();

        }



        public void AddOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            if (Orders.Contains(order.Id))
            {
                throw new ArgumentException($"Order already added");
            }

            Orders.Add(order.Id);
            if (order.Reservations != null) TotalPrice += order.TotalAmount ?? 0;
        }

        public void AddMoviePass(MoviePass moviePass)
        {
            if (moviePass == null)
            {
                throw new ArgumentNullException(nameof(moviePass));
            }

            if (MoviePasses.Contains(moviePass.Id))
            {
                throw new ArgumentException($"Movie pass already added");
            }

            MoviePasses.Add(moviePass.Id);
            TotalPrice += moviePass.Price;
        }

        public void AddSubscription(Subscription subscription)
        {
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            if (Subscriptions.Contains(subscription.Id))
            {
                throw new ArgumentException($"Subscription already added");
            }

            Subscriptions.Add(subscription.Id);
            TotalPrice += subscription.Price;
        }

        public void Save()
        {
            _session.SetString("cart_orders", JsonConvert.SerializeObject(Orders));
            _session.SetString("cart_subscriptions", JsonConvert.SerializeObject(Subscriptions));
            _session.SetString("cart_movie_passes", JsonConvert.SerializeObject(Subscriptions));
        }
    }
}
