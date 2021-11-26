using Newtonsoft.Json;
using PinkPanther.BlueCrocodile.Core.Models;
using PinkPanther.BlueCrocodile.Infrastructure.Data;
using PinkPanther.BlueCrocodile.Seed.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.Seed
{
    internal class Seeder
    {
        private readonly ArrangementRepository _arrangementRepository;
        private readonly DiscountRepository _discountRepository;
        private readonly MovieRepository _movieRepository;
        private readonly OrderRepository _orderRepository;

        public List<Seat> SeedSeats = new List<Seat>
        {
            new Seat {Row = 1, SeatNumber = 1},
            new Seat {Row = 1, SeatNumber = 2},
            new Seat {Row = 1, SeatNumber = 3},
            new Seat {Row = 1, SeatNumber = 4},
            new Seat {Row = 1, SeatNumber = 5},
            new Seat {Row = 1, SeatNumber = 6},
            new Seat {Row = 1, SeatNumber = 7},
            new Seat {Row = 1, SeatNumber = 8},
            new Seat {Row = 2, SeatNumber = 1},
            new Seat {Row = 2, SeatNumber = 2},
            new Seat {Row = 2, SeatNumber = 3},
            new Seat {Row = 2, SeatNumber = 4},
            new Seat {Row = 2, SeatNumber = 5},
            new Seat {Row = 2, SeatNumber = 6},
            new Seat {Row = 2, SeatNumber = 7},
            new Seat {Row = 2, SeatNumber = 8},
            new Seat {Row = 3, SeatNumber = 1},
            new Seat {Row = 3, SeatNumber = 2},
            new Seat {Row = 3, SeatNumber = 3},
            new Seat {Row = 3, SeatNumber = 4},
            new Seat {Row = 3, SeatNumber = 5},
            new Seat {Row = 3, SeatNumber = 6},
            new Seat {Row = 3, SeatNumber = 7},
            new Seat {Row = 3, SeatNumber = 8}
        };

        public Seeder(string connectionString)
        {
            var dataContext = new DataContext(connectionString);
            _movieRepository = new MovieRepository(dataContext);
            _orderRepository = new OrderRepository(dataContext, _movieRepository);
            _discountRepository = new DiscountRepository(dataContext);
            _arrangementRepository = new ArrangementRepository(dataContext);
        }

        public async Task SeedAsync()
        {
            await SeedMoviesAsync();
            await RemoveOrdersAsync();
            await SeedDiscountsAsync();
            await SeedArrangementsAsync();
        }

        private async Task SeedArrangementsAsync()
        {
            var arrangements = new List<Arrangement>
            {
                new Arrangement {Name = "Popcorn", Ammount = 1.00m, Description = "Popcorn arrangement."}
            };

            await _arrangementRepository.RemoveAllAsync();
            await _arrangementRepository.InsertManyAsync(arrangements);
        }

        private async Task SeedDiscountsAsync()
        {
            var discounts = new List<Discount>
            {
                new Discount
                {
                    Name = "Child", Ammount = 1.50m, Description = "De korting is geldig voor kinderen tot en " +
                                                                   "met 11 jaar. De korting geldt alleen op de " +
                                                                   "voorstellingen tot 18:00 uur voor " +
                                                                   "Nederlands gesproken kinderfilms."
                },
                new Discount
                {
                    Name = "Student", Ammount = 1.50m, Description = "Op vertoon van een studentenkaart wordt " +
                                                                     "korting gegeven. De korting is alleen geldig " +
                                                                     "van maandag t/m donderdag."
                },
                new Discount
                {
                    Name = "65+", Ammount = 1.50m, Description = "Op vertoon van de 65+ pas wordt korting " +
                                                                 "gegeven. De korting is alleen geldig van " +
                                                                 "maandag t/m donderdag en geldt niet in " +
                                                                 "vakanties en/of feestdagen."
                }
            };

            await _discountRepository.RemoveAllAsync();
            await _discountRepository.InsertManyAsync(discounts);
        }

        private async Task SeedMoviesAsync()
        {
            string json;

            using (var r = File.OpenText("movies.json"))
            {
                json = r.ReadToEnd();
            }

            var items = JsonConvert.DeserializeObject<List<MovieSeed>>(json);
            var movies = items.Select(m => new Movie
                {
                    Title = m.Title,
                    Description = m.Description,
                    Image = m.Image,
                    ShowTimes = GenerateRandomShowTimes()
                })
                .ToList();

            await _movieRepository.RemoveAllAsync();
            await _movieRepository.InsertManyAsync(movies);
        }

        private async Task RemoveOrdersAsync()
        {
            await _orderRepository.RemoveAllAsync();
        }

        private IEnumerable<ShowTime> GenerateRandomShowTimes()
        {
            var r = new Random();

            var result = new List<ShowTime>();

            for (var i = 0; i < r.Next(0, 20); i++)
            {
                var randomDays = r.Next(-100, 100);
                var randomSeconds = r.Next(0, 86400);
                var randomDateTime = DateTime.Now.AddDays(randomDays).AddSeconds(randomSeconds);

                result.Add(new ShowTime
                {
                    Id = i.ToString(),
                    DateTime = randomDateTime,
                    Room = new Room
                    {
                        Number = r.Next(1, 10),
                        Seats = SeedSeats,
                        WeelChairAccessable = r.Next(0, 2) == 0
                    }
                });
            }

            return result;
        }
    }
}