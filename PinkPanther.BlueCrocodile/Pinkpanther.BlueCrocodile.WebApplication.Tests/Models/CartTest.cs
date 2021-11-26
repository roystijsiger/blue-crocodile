using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PinkPanther.BlueCrocodile.Core.Models;
using PinkPanther.BlueCrocodile.WebApplication.Models;
using PinkPanther.BlueCrocodile.WebApplication.Tests.Mocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace PinkPanther.BlueCrocodile.WebApplication.Tests.Models
{

    public class CartTest
    {
        [Fact]
        public void add_order_should_have_order()
        {
            // Arrange
            var mockSession = new MockSession();
            var testOrder = new Order
            {
                Id = Guid.NewGuid().ToString()
            };


            // Act
            var sut = new Cart(mockSession);
            sut.AddOrder(testOrder);


            // Assert
            Assert.Contains(sut.Orders, o => o == testOrder.Id);
        }

        [Fact]
        public void add_null_order_should_throw_exception()
        {
            // Arrange
            var mockSession = new MockSession();
            Order testOrder = null;


            // Act
            var sut = new Cart(mockSession);
            Action act = () => sut.AddOrder(testOrder);


            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void add_duplicate_order_should_throw_exception()
        {
            // Arrange
            var mockSession = new MockSession();
            var testOrder = new Order
            {
                Id = Guid.NewGuid().ToString()
            };


            // Act
            var sut = new Cart(mockSession);
            sut.AddOrder(testOrder);
            Action act = () => sut.AddOrder(testOrder);


            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void add_subscription_should_have_subscription()
        {
            // Arrange
            var mockSession = new MockSession();
            var testSubscription = new Subscription
            {
                Id = Guid.NewGuid().ToString()
            };


            // Act
            var sut = new Cart(mockSession);
            sut.AddSubscription(testSubscription);


            // Assert
            Assert.Contains(sut.Subscriptions, o => o == testSubscription.Id);
        }

        [Fact]
        public void add_null_subscription_should_throw_exception()
        {
            // Arrange
            var mockSession = new MockSession();
            Subscription testSubscription = null;


            // Act
            var sut = new Cart(mockSession);
            Action act = () => sut.AddSubscription(testSubscription);


            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void add_duplicate_subscription_should_throw_exception()
        {
            // Arrange
            var mockSession = new MockSession();
            var testSubscription = new Subscription
            {
                Id = Guid.NewGuid().ToString()
            };


            // Act
            var sut = new Cart(mockSession);
            sut.AddSubscription(testSubscription);
            Action act = () => sut.AddSubscription(testSubscription);


            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void add_movie_pass_should_have_subscription()
        {
            // Arrange
            var mockSession = new MockSession();
            var testMoviePass = new MoviePass()
            {
                Id = Guid.NewGuid().ToString()
            };


            // Act
            var sut = new Cart(mockSession);
            sut.AddMoviePass(testMoviePass);


            // Assert
            Assert.Contains(sut.MoviePasses, o => o == testMoviePass .Id);
        }

        [Fact]
        public void add_null_movie_pass_should_throw_exception()
        {
            // Arrange
            var mockSession = new MockSession();
            MoviePass testMoviePass = null;


            // Act
            var sut = new Cart(mockSession);
            Action act = () => sut.AddMoviePass(testMoviePass);


            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void add_duplicate_movie_pass_should_throw_exception()
        {
            // Arrange
            var mockSession = new MockSession();
            var testMoviePass  = new MoviePass()
            {
                Id = Guid.NewGuid().ToString()
            };


            // Act
            var sut = new Cart(mockSession);
            sut.AddMoviePass(testMoviePass);
            Action act = () => sut.AddMoviePass(testMoviePass);


            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void save_should_store_cart()
        {
            // Arrange
            var mockSession = new MockSession();
            var testOrder = new Order { Id = Guid.NewGuid().ToString() };
            var testSubscription = new Subscription { Id = Guid.NewGuid().ToString() };


            // Act
            var sut = new Cart(mockSession);
            sut.AddOrder(testOrder);
            sut.AddSubscription(testSubscription);
            sut.Save();


            // Assert
            var reservationsJson = mockSession.GetString("cart_orders");
            Assert.NotNull(reservationsJson);

            var orders = JsonConvert.DeserializeObject<List<string>>(reservationsJson);
            Assert.Contains(testOrder.Id, orders);


            var subscriptionsJson = mockSession.GetString("cart_subscriptions");
            Assert.NotNull(subscriptionsJson);

            var subscriptions = JsonConvert.DeserializeObject<List<string>>(subscriptionsJson);
            Assert.Contains(testSubscription.Id, subscriptions);
        }

        [Fact]
        public void instantiate_should_load_cart()
        {
            // Arrange
            var mockSession = new MockSession();

            var testOrders = new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
            var testSubscriptions = new List<string> { Guid.NewGuid().ToString() };

            mockSession.SetString("cart_orders", JsonConvert.SerializeObject(testOrders));
            mockSession.SetString("cart_subscriptions", JsonConvert.SerializeObject(testSubscriptions));


            // Act
            var sut = new Cart(mockSession);


            // Assert
            Assert.Contains(sut.Orders, o => o == testOrders[0]);
            Assert.Contains(sut.Orders, o => o == testOrders[1]);
            Assert.Contains(sut.Subscriptions, o => o == testSubscriptions[0]);
        }

        [Fact]
        public void total_amount_should_return_total()
        {
            // Arrange
            var mockSession = new MockSession();
            var testOrder = new Order { Reservations = new List<Reservation> { new Reservation { Price = 14 } } };
            var testSubscription = new Subscription { Price = 100 };


            // Act
            var sut = new Cart(mockSession);
            sut.AddOrder(testOrder);
            sut.AddSubscription(testSubscription);
            var total = sut.TotalPrice;


            // Assert
            Assert.Equal(114, total);
        }
    }
}
