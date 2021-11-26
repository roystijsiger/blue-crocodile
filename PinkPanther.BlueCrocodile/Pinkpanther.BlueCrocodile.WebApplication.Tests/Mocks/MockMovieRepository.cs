using PinkPanther.BlueCrocodile.Core.Models;
using PinkPanther.BlueCrocodile.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.WebApplication.Tests.Mocks
{
    public class MockMovieRepository : IMovieRepository
    {
        private IEnumerable<Movie> _movies;

        public MockMovieRepository()
        {
            var movies = new List<Movie>();
            // movie 1
            var movie1 = new Movie();
            movie1.Id = "1";
            movie1.Title = "Movie 1";
            movie1.Description = "Movie 1 description";
            movie1.Image = "movie1.png";

            // showtimes
            var movie1ShowTime1 = new ShowTime();
            movie1ShowTime1.Id = "1";
            movie1ShowTime1.Room = new Room
            {
                Number = 1,
                Seats = new List<Seat>(),
                WeelChairAccessable = true
            };

            movie1ShowTime1.DateTime = DateTime.Now.AddDays(1);

            var movie1ShowTime2 = new ShowTime();
            movie1ShowTime2.Id = "2";
            movie1ShowTime2.Room = new Room
            {
                Number = 2,
                Seats = new List<Seat>(),
                WeelChairAccessable = true
            };
            movie1ShowTime2.DateTime = DateTime.Now.AddDays(2);

            List<ShowTime> Movie1ShowTimes = new List<ShowTime>();
            Movie1ShowTimes.Add(movie1ShowTime1);
            Movie1ShowTimes.Add(movie1ShowTime2);

            //show time to movie
            movie1.ShowTimes = Movie1ShowTimes;

            //movie2 
            var movie2 = new Movie();
            movie2.Id = "2";
            movie2.Title = "John Wick II";
            movie2.Description = "Assassin John Wick has a debt and has to pay it will he or will he not?!?!?!?!?!?!";
            movie2.Image = "JWII.png";

            //movie 2 show times
            var movie2ShowTime1 = new ShowTime();
            movie2ShowTime1.Id = "3";
            movie2ShowTime1.DateTime = DateTime.Now.AddDays(9);
            movie2ShowTime1.Room = new Room
            {
                Number = 2,
                Seats = new List<Seat>(),
                WeelChairAccessable = true
            };

            var movie2ShowTime2 = new ShowTime();

            movie2ShowTime2.Id = "4";
            movie2ShowTime2.Room = new Room
            {
                Number = 5,
                Seats = new List<Seat>(),
                WeelChairAccessable = true
            };
            movie2ShowTime2.DateTime = DateTime.Now.AddDays(18);

            var movie2ShowTimes = new List<ShowTime>();
            movie2ShowTimes.Add(movie2ShowTime1);
            movie2ShowTimes.Add(movie2ShowTime2);

            movie2.ShowTimes = movie2ShowTimes;

            //add movies to list
            movies.Add(movie1);
            movies.Add(movie2);

            _movies = movies;
        }

        public MockMovieRepository(IEnumerable<Movie> movies)
        {
            _movies = movies;
        }

        public Task<IEnumerable<Movie>> GetAllAsync()
        {
            return Task.FromResult(_movies);
        }

        public Task<Movie> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> InsertAsync(Movie item)
        {
            throw new NotImplementedException();
        }

        public Task InsertManyAsync(List<Movie> orders)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> UpdateAsync(Movie item)
        {
            throw new NotImplementedException();
        }
    }
}

/*
 * 
            
            {
                new Movie
                {
                    Id = "1",
                    Title = "Movie 1",
                    Description = "Movie description 1",
                    Image = "http://example.com/movie1.png",
                    ShowTimes = new List<ShowTime>
                    {
                        new ShowTime
                        {
                            Id = "1",
                            DateTime = DateTime.Now.AddDays(1),
                             RoomId = new RoomId
                            {
                                Number = 1,
                                WeelChairAccessable = false,
                                Seats =
                                {
                                    new Seat
                                    {
                                        Location = "1a1",
                                        Row = 1,
                                        SeatNumber = 1
                                    },
                                    new Seat
                                    {
                                        Location = "1a2",
                                        Row = 1,
                                        SeatNumber = 2
                                    },
                                }
                            }
                        },
                        new ShowTime
                        {
                            Id = "2",
                            DateTime = DateTime.Now.AddDays(7),
                            RoomId = new RoomId
                            {
                                Number = 2,
                                WeelChairAccessable = false,
                                Seats =
                                {
                                    new Seat
                                    {
                                        Location = "1a1",
                                        Row = 1,
                                        SeatNumber = 1
                                    },
                                    new Seat
                                    {
                                        Location = "1a2",
                                        Row = 1,
                                        SeatNumber = 2
                                    },
                                }
                            }
                        }
                    }
                },
                new Movie
                {
                    Id = "2",
                    Title = "Movie 2",
                    Description = "Movie description 2",
                    Image = "http://example.com/movie2.png",
                    ShowTimes = new List<ShowTime>
                    {
                        new ShowTime
                        {
                            Id = "1",
                            DateTime = DateTime.Now.AddDays(30),
                            RoomId = new RoomId
                            {
                                Number = 3,
                                WeelChairAccessable = false,
                                Seats =
                                {
                                    new Seat
                                    {
                                        Location = "1a1",
                                        Row = 1,
                                        SeatNumber = 1
                                    },
                                    new Seat
                                    {
                                        Location = "1a2",
                                        Row = 1,
                                        SeatNumber = 2
                                    },
                                }
                            }
                        },
                        new ShowTime
                        {
                            Id = "2",
                            DateTime = DateTime.Now.AddDays(31),
                            RoomId = new RoomId
                            {
                                Number = 3,
                                WeelChairAccessable = false,
                                Seats =
                                {
                                    new Seat
                                    {
                                        Location = "1a1",
                                        Row = 1,
                                        SeatNumber = 1
                                    },
                                    new Seat
                                    {
                                        Location = "1a2",
                                        Row = 1,
                                        SeatNumber = 2
                                    },
                                }
                            }
                        }
                    }
                }
            };
            */
