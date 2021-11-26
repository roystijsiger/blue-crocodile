using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using PinkPanther.BlueCrocodile.Core.Models;
using PinkPanther.BlueCrocodile.Core.Repositories;
using PinkPanther.BlueCrocodile.WebApplication.Areas.Website.ViewModels;
using PinkPanther.BlueCrocodile.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PaymentMethod = PinkPanther.BlueCrocodile.Core.Models.PaymentMethod;
using PaymentStatus = PinkPanther.BlueCrocodile.Core.Models.PaymentStatus;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.Website.Controllers
{
    [Area("Website")]
    public class OrdersController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMovieRepository _movieRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IArrangementRepository _arrangementRepository;
        private readonly IPaymentClient _paymentClient;

        public OrdersController(IHostingEnvironment hostingEnvironment, IMovieRepository movieRepository, IOrderRepository orderRepository, IPaymentClient paymentClient, IArrangementRepository arrangementRepository, IDiscountRepository discountRepository)
        {
            _hostingEnvironment = hostingEnvironment;
            _movieRepository = movieRepository;
            _orderRepository = orderRepository;
            _paymentClient = paymentClient;
            _discountRepository = discountRepository;
            _arrangementRepository = arrangementRepository;
        }

        public async Task<IActionResult> Create(string movieId, string showTimeId)
        {
            var movie = await _movieRepository.GetAsync(movieId);
            var showTime = movie.ShowTimes.Single(s => s.Id == showTimeId);

            var order = new Order()
            {
                Code = Guid.NewGuid().ToString().Substring(0, 6),
                Movie = movie,
                ShowTime = showTime
            };

            var insertedOrder = await _orderRepository.InsertAsync(order);

            var viewModel = new OrderViewModel(insertedOrder, _orderRepository);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateRandomPlacedReservation([FromForm] OrderViewModel model)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        public async Task<IActionResult> Place([FromForm] OrderViewModel model)
        {
            var order = await _orderRepository.GetAsync(model.Id);

            order.Reservations = order.Reservations ?? new List<Reservation>();

            // Add adult reservations
            for (var i = 0; i < model.AmountOfAdults; i++)
            {
                order.Reservations.Add(new Reservation
                {
                    Seat = new Seat(),
                    Price = 15
                });
            }

            // Add child reservations
            for (var i = 0; i < model.AmountOfChildren; i++)
            {
                order.Reservations.Add(new Reservation
                {
                    Seat = new Seat(),
                    Price = 9
                });
            }

            var savedOrder = await _orderRepository.UpdateAsync(order);

            return RedirectToAction(nameof(DiscountsAndArrangements), new { orderId = savedOrder.Id });
        }

        [HttpGet]
        public async Task<IActionResult> DiscountsAndArrangements(string orderId)
        {
            var order = await _orderRepository.GetAsync(orderId);
            var orderViewModel = new OrderViewModel(order, _orderRepository);

            orderViewModel.DiscountsAvailable = await _discountRepository.GetAllAsync();
            orderViewModel.ArrangementsAvailable = await _arrangementRepository.GetAllAsync();

            return View(orderViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDiscounts([FromForm]OrderViewModel model)
        {
            var order = await _orderRepository.GetAsync(model.Id);
            var discounts = await _discountRepository.GetAllAsync();
            var arrangements = await _arrangementRepository.GetAllAsync();
            for (int i = 0; i < order.Reservations.Count; i++)
            {
                order.Reservations[i].Arrangement = model.Reservations[i].Arrangement;
                order.Reservations[i].Discount = model.Reservations[i].Discount;
            }

            foreach (var reservation in order.Reservations)
            {
                if (reservation.Arrangement == null)
                {
                    reservation.Arrangement = new Arrangement() { Name = "No arrangement" };
                }

                if (reservation.Discount == null)
                {
                    reservation.Discount = new Discount() { Name = "Normal"};
                }

                reservation.Discount = discounts.FirstOrDefault(x => x.Name.Equals(reservation.Discount.Name));
                reservation.Arrangement = arrangements.FirstOrDefault(x => x.Name.Equals(reservation.Arrangement.Name));
            }

            for (int i = 0; i < order.Reservations.Count; i++)
            {
                var thisReservation = order.Reservations[i];
                thisReservation.Price = 9.00m;
                if (thisReservation.Discount != null)
                {
                    thisReservation.Price -= thisReservation.Discount.Ammount;
                }
                if (thisReservation.Arrangement != null)
                {
                    thisReservation.Price += thisReservation.Arrangement.Ammount;
                }
            }

            var savedOrder = await _orderRepository.UpdateAsync(order);
            return RedirectToAction(nameof(SeatSelection), new { orderId = savedOrder.Id });
        }

        [HttpGet]
        public async Task<IActionResult> SeatSelection(string orderId)
        {
            var order = await _orderRepository.GetAsync(orderId);
            var orderViewModel = new OrderViewModel(order, _orderRepository);
        
            return View(orderViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmSeatSelection([FromForm]OrderViewModel model)
        {
            var order = await _orderRepository.GetAsync(model.Id);

            var orderViewModel = new OrderViewModel(order, _orderRepository);

            var unselectedSeats = new List<int>();
            var takenSeats = new List<Seat>();
            for (var i = 0; i < model.Reservations.Count; i++)
            {
                //check if a seat isnt filled in
                //if(model.Reservations[i].Seat.Location.Equals("--Select Seat--"))
                if (model.Reservations[i].Seat.Location == null)
                {
                    //amount of empty seats +1
                    unselectedSeats.Add(i);
                    continue;
                }
                
                var currentSeatLocation = model.Reservations[i].Seat.Location;
                
                var seat = new Seat()
                {
                    Row = Int32.Parse(currentSeatLocation.Substring(0, currentSeatLocation.IndexOf("-"))),
                    SeatNumber = Int32.Parse(currentSeatLocation.Substring(currentSeatLocation.IndexOf("-") + 1))
                };

                //add seat to takenSeats list
                takenSeats.Add(seat);
                model.Reservations[i].Seat = seat;
                model.Reservations[i].Price = order.Reservations[i].Price;
            }
            
            //see if there are unselected seats

            var foundSeats = false;
            if (unselectedSeats.Count > 0)
            {
                var seats = orderViewModel.AvailableSeats();

                //group all available seats by row :)
                var seatsGroupedByRow = seats.GroupBy(g => g.Row);

                //loop through rows
                foreach (var sg in seatsGroupedByRow)
                {

                    if (foundSeats)
                    {
                        break;
                    }

                    var seatsInARowInRow = 1;

                    var lastSeat = new Seat
                    {
                        Location = "0",
                        Row = 0,
                        SeatNumber = 0
                    };

                    //loop through seeds in row
                    foreach (var s in sg)
                    {


                        //if the last seat number is 0 then its the first loop
                        if(lastSeat.SeatNumber == 0)
                        {
                            lastSeat = s;
                            continue;
                        }

                        //if the lastseat + 1 is the same as the current seatnumber seatsinarow +1
                        if(lastSeat.SeatNumber + 1 == s.SeatNumber && lastSeat.Row == sg.Key)
                        {
                            //seats in a row ++ :)
                            seatsInARowInRow++;

                            //set last seat
                            lastSeat = s;

                            //means we have enough in a row seatwise
                            if (seatsInARowInRow == unselectedSeats.Count)
                            {
                                var i = unselectedSeats.Count;
                                foreach(int u in unselectedSeats)
                                {
                                    //seatnumber calculation
                                    var seatNumber = i;

                                    //create a new seat
                                    model.Reservations[u].Seat = new Seat
                                    {
                                        Location = sg.Key + "-" + seatNumber,
                                        Row = sg.Key,
                                        SeatNumber = seatNumber
                                    };

                                    //remove the seats from unselected cause now its selected :)


                                    //i++ for calculating the seat:)
                                    i--;
                                }

                                foundSeats = true;
                                break;
                            }
                        }
                        else
                        {

                            lastSeat = s;
                            seatsInARowInRow = 1;
                        }
                    }
                }

                //couldnt find any available seats in a row in a row :(
                if (!foundSeats)
                {
                    for(var i = 0; i < unselectedSeats.Count; i++)
                    {
                        model.Reservations[unselectedSeats[i]].Seat = new Seat
                        {
                            Location = seats[i].Location,
                            Row = seats[i].Row,
                            SeatNumber = seats[i].SeatNumber
                        };

                    }
                }
            }

            for (int i = 0; i < order.Reservations.Count; i++)
            {
                order.Reservations[i].Seat = model.Reservations[i].Seat;
            }

            var savedOrder = await _orderRepository.UpdateAsync(order);

            return RedirectToAction(nameof(OrderConfirmation), new { orderId = savedOrder.Id });
        }

        [HttpGet]
        public async Task<IActionResult> OrderConfirmation(string orderId)
        {
            var order = await _orderRepository.GetAsync(orderId);
            var orderViewModel = new OrderViewModel(order, _orderRepository);

            return View(orderViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> StartPayment([FromForm] OrderViewModel orderViewModel)
        {
            var order = await _orderRepository.GetAsync(orderViewModel.Id);
            order.Email = orderViewModel.Email;
            order.PaymentMethod = orderViewModel.PaymentMethod;
            order.PaymentStatus = PaymentStatus.Open;

            if (orderViewModel.PaymentMethod == PaymentMethod.AtCounter)
            {
                await _orderRepository.UpdateAsync(order);
                return RedirectToAction("PaymentConfirmed", new {orderId = order.Id});
            }

            var scheme = Url.ActionContext.HttpContext.Request.Scheme;
            var request = new PaymentRequest
            {
                Amount = new Amount(Currency.EUR, order.TotalAmount?.ToString("F", CultureInfo.InvariantCulture)),
                Description = $"Reservation {order.Movie.Title} - {order.ShowTime.DateTime}",
                RedirectUrl = Url.Action("PaymentConfirmed", "Orders", new { orderId = order.Id }, scheme),
            };

            switch (order.PaymentMethod)
            {
                case PaymentMethod.IDeal:
                    request.Method = Mollie.Api.Models.Payment.PaymentMethod.Ideal;
                    break;
                case PaymentMethod.CreditCard:
                    request.Method = Mollie.Api.Models.Payment.PaymentMethod.CreditCard;
                    break;
            }

            if (_hostingEnvironment.IsProduction())
            {
                request.WebhookUrl = Url.Action("PaymentWebhook", "Orders", null, scheme);
            }

            request.SetMetadata(new PaymentMetadata { OrderId = order.Id, Email = order.Email });

            var payment = await _paymentClient.CreatePaymentAsync(request);
            order.PaymentId = payment.Id;
            await _orderRepository.UpdateAsync(order);

            return Redirect(payment.Links.Checkout.Href);

        }

        [HttpPost]
        public async Task<IActionResult> PaymentWebhook([FromForm]string id)
        {
            var payment = await _paymentClient.GetPaymentAsync(id);
            var metadata = payment.GetMetadata<PaymentMetadata>();

            var order = await _orderRepository.GetAsync(metadata.OrderId);
            order.PaymentStatus = (PaymentStatus) payment.Status;

            if (order.PaymentStatus == PaymentStatus.Paid)
            {
                foreach (var reservation in order.Reservations)
                {
                    reservation.Code = Guid.NewGuid().ToString().Substring(0,12);
                }
            }

            await _orderRepository.UpdateAsync(order);

            return Ok("OK");
        }

        [HttpGet]
        public async Task<IActionResult> PaymentConfirmed(string orderId)
        {
            var order = await _orderRepository.GetAsync(orderId);
            if (order.PaymentStatus != PaymentStatus.Paid && order.PaymentMethod != PaymentMethod.AtCounter)
            {
                await PaymentWebhook(order.PaymentId);
                order = await _orderRepository.GetAsync(orderId);
            }

            var orderViewModel = new OrderViewModel(order, _orderRepository);

            return View("PrintTickets", orderViewModel);
        }
    }
}