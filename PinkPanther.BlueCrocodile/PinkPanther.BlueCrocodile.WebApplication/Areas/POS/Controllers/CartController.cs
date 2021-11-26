using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinkPanther.BlueCrocodile.Core.Models;
using PinkPanther.BlueCrocodile.Core.Repositories;
using PinkPanther.BlueCrocodile.WebApplication.Areas.POS.ViewModels;
using PinkPanther.BlueCrocodile.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.POS.Controllers
{
    [Area("POS")]
    public class CartController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IArrangementRepository _arrangementRepository;
        private readonly IMoviePassRepository _moviePassRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;


        public CartController(
            IMovieRepository movieRepository, 
            IOrderRepository orderRepository, 
            IArrangementRepository arrangementRepository, 
            IDiscountRepository discountRepository,
            IMoviePassRepository moviePassRepository,
            ISubscriptionRepository subscriptionRepository)
        {
            _movieRepository = movieRepository;
            _orderRepository = orderRepository;
            _discountRepository = discountRepository;
            _arrangementRepository = arrangementRepository;
            _moviePassRepository = moviePassRepository;
            _subscriptionRepository = subscriptionRepository;

        }

        public async Task<IActionResult> Overview()
        {
            
            var viewModel = new CartOverviewViewModel
            {
                Showtimes = await GetMovieShowTimes(),
                Cart = new Cart(HttpContext.Session)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct([FromForm] CartOverviewViewModel model)
        {
            var cart = new Cart(HttpContext.Session);
            model.Cart = cart;
            model.Showtimes = await GetMovieShowTimes();

            if (!ModelState.IsValid)
            {
                return View("Overview", model);
            }

            var action = model.Action switch
            {
                "Add reservation" => CartOverviewAction.AddReservation,
                "Add subscription" => CartOverviewAction.AddSubscription,
                "Add movie pass" => CartOverviewAction.AddMoviePass,

                _ => CartOverviewAction.Unknown
            };

            switch (action)
            {
                case CartOverviewAction.Unknown:
                    ModelState.AddModelError("InvalidAction", "The provided action is invalid");
                    break;
                    

                case CartOverviewAction.AddMoviePass:
                    var pass = new MoviePass
                    {
                        Price = 50
                    };
                    var insertedPass = await _moviePassRepository.InsertAsync(pass);
                    cart.AddMoviePass(insertedPass);


                    break;
                        

                case CartOverviewAction.AddSubscription:
                    var subscription = new Subscription
                    {
                        Price = 200
                    };
                    var insertedSubscriptions = await _subscriptionRepository.InsertAsync(subscription);
                    cart.AddSubscription(insertedSubscriptions);

                    break;
            }

            cart.Save();


            return View("Overview", model);

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

            var viewModel = new Website.ViewModels.OrderViewModel(insertedOrder, _orderRepository);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Place([FromForm] Website.ViewModels.OrderViewModel model)
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

        [HttpPost]
        public async Task<IActionResult> Place([FromForm] OrderViewModel model)
        {
            var order = await _orderRepository.GetAsync(model.Id);

            order.Reservations ??= new List<Reservation>();

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
                    reservation.Discount = new Discount() { Name = "Normal" };
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
                        if (lastSeat.SeatNumber == 0)
                        {
                            lastSeat = s;
                            continue;
                        }

                        //if the lastseat + 1 is the same as the current seatnumber seatsinarow +1
                        if (lastSeat.SeatNumber + 1 == s.SeatNumber && lastSeat.Row == sg.Key)
                        {
                            //seats in a row ++ :)
                            seatsInARowInRow++;

                            //set last seat
                            lastSeat = s;

                            //means we have enough in a row seatwise
                            if (seatsInARowInRow == unselectedSeats.Count)
                            {
                                var i = unselectedSeats.Count;
                                foreach (int u in unselectedSeats)
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
                    for (var i = 0; i < unselectedSeats.Count; i++)
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

        private async Task<IEnumerable<IGrouping<string, MovieShowTimeViewModel>>> GetMovieShowTimes()
        {
            var response = await _movieRepository.GetAllAsync();

            var moviesShowTime = new List<MovieShowTimeViewModel>();
            foreach (var m in response)
            {
                moviesShowTime.AddRange(m.ShowTimes.Select(st => new MovieShowTimeViewModel
                {
                    Id = m.Id, 
                    Title = m.Title, 
                    ShowTime = st.DateTime, 
                    Room = st.Room.Number
                }));
            }


            return moviesShowTime
                .Where(s => s.ShowTime.Date >= DateTime.Now)
                .OrderByDescending(m => m.ShowTime)
                .GroupBy(s => s.Title);
        }


    }
}