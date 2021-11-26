using Microsoft.AspNetCore.Mvc;
using PinkPanther.BlueCrocodile.Core.Models;
using PinkPanther.BlueCrocodile.Core.Repositories;
using PinkPanther.BlueCrocodile.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.Website.Controllers
{
    [Area("Website")]
    [Route("[controller]/[action]")]
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Shows the details of a selected movie
        /// </summary>
        /// <param name="Id">Id of the movie</param>
        /// <returns>View with the movie details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string Id)
        {

            Movie movie = await _movieRepository.GetAsync(Id);
            IEnumerable<ShowTime> showTimes = movie.ShowTimes;
            movie.ShowTimes = showTimes.Where(s => s.DateTime > DateTime.Now).OrderBy(o => o.DateTime).ToList();
            return View(movie);
        }


        [HttpPost]
        public async Task<IActionResult> ByWeek(int roomNumber, DateTime fromDate, DateTime tillDate)
        {
            IEnumerable<Movie> movies = await _movieRepository.GetAllAsync();

            //create a new list
            List<MovieDetailsShowTimeView> moviesShowTime = new List<MovieDetailsShowTimeView>();

            //foreach through the movies and showtimes. For every showtime add an entry to the list<movieshowtimeviewmodel>
            foreach (Movie m in movies)
            {
                foreach (ShowTime st in m.ShowTimes)
                {
                    MovieDetailsShowTimeView movieShowTime = new MovieDetailsShowTimeView();
                    movieShowTime.MovieId = m.Id;
                    movieShowTime.ShowTimeId = st.Id;
                    movieShowTime.Title = m.Title;
                    movieShowTime.Description = m.Description;
                    movieShowTime.Image = m.Image;
                    movieShowTime.DateTime = st.DateTime;
                    movieShowTime.Room = st.Room.Number;
                    moviesShowTime.Add(movieShowTime);
                }
            }

            IEnumerable<MovieDetailsShowTimeView> moviesFiltered = moviesShowTime;
            if (roomNumber > 0)
            {
                moviesFiltered = moviesFiltered.Where(m => m.Room == roomNumber).ToList();
            }

            if (fromDate != null && fromDate != DateTime.MinValue)
            {
                moviesFiltered = moviesFiltered.Where(m => m.DateTime >= fromDate).ToList();
            }

            if (tillDate != null && tillDate != DateTime.MinValue)
            {
                moviesFiltered = moviesFiltered.Where(m => m.DateTime <= tillDate).ToList();
            }
            //get all the showtimes between now and +7 days and only leave 1 entry per movie
            //List<MovieDetailsShowTimeView> moviesThisWeek = moviesShowTime.Where(m => m.DateTime >= DateTime.Now && m.DateTime <= DateTime.Now.AddDays(7)).GroupBy(m => m.Title).Select(g => g.First()).ToList();
            //
            ViewData["title"] = String.Format(
                "Filtered From: {0} Till: {1} RoomId: {2}",
                fromDate != null && fromDate != DateTime.MinValue ? fromDate.ToString() : "-",
                tillDate != null && tillDate != DateTime.MinValue ? tillDate.ToString() : "-",
                roomNumber > 0 ? roomNumber.ToString() : "-"
            );

            return View(moviesFiltered.GroupBy(m => m.Title).Select(g => g.First()).ToList());
        }
        /// <summary>
        /// Shows all movies that are available in the next 7 days
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ByWeek()
        {
            ViewData["title"] = "Movies in the upcoming 7 days";
            //get all the movie times
            IEnumerable<Movie> movies = await _movieRepository.GetAllAsync();

            //create a new list
            List<MovieDetailsShowTimeView> moviesShowTime = new List<MovieDetailsShowTimeView>();

            //foreach through the movies and showtimes. For every showtime add an entry to the list<movieshowtimeviewmodel>
            foreach (Movie m in movies)
            {
                foreach (ShowTime st in m.ShowTimes)
                {
                    MovieDetailsShowTimeView movieShowTime = new MovieDetailsShowTimeView();
                    movieShowTime.MovieId = m.Id;
                    movieShowTime.ShowTimeId = st.Id;
                    movieShowTime.Title = m.Title;
                    movieShowTime.Description = m.Description;
                    movieShowTime.Image = m.Image;
                    movieShowTime.DateTime = st.DateTime;
                    movieShowTime.Room = st.Room.Number;
                    moviesShowTime.Add(movieShowTime);
                }
            }

            //get all the showtimes between now and +7 days and only leave 1 entry per movie
            List<MovieDetailsShowTimeView> moviesThisWeek = moviesShowTime.Where(m => m.DateTime >= DateTime.Now && m.DateTime <= DateTime.Now.AddDays(7)).GroupBy(m => m.Title).Select(g => g.First()).ToList();

            
            return View(moviesThisWeek);
        }
    }
}