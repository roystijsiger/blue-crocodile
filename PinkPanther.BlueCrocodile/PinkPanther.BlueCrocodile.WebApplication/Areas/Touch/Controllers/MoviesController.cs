using Microsoft.AspNetCore.Mvc;
using PinkPanther.BlueCrocodile.Core.Models;
using PinkPanther.BlueCrocodile.Core.Repositories;
using PinkPanther.BlueCrocodile.WebApplication.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.Touch.Controllers
{
    [Area("Touch")]
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        
        public async Task<ActionResult> Index()
        {
            var response = await _movieRepository.GetAllAsync();

            List<MovieShowTimeView> moviesShowTime = new List<MovieShowTimeView>();
            foreach(Movie m in response)
            {
                foreach(ShowTime st in m.ShowTimes) {
                    MovieShowTimeView movieShowTime = new MovieShowTimeView();
                    movieShowTime.Id = m.Id;
                    movieShowTime.Title = m.Title;
                    movieShowTime.ShowTime = st.DateTime;
                    movieShowTime.Room = st.Room.Number;
                    moviesShowTime.Add(movieShowTime);
                }
            }

            return View(moviesShowTime.OrderByDescending(m => m.ShowTime));
        }
        
        public async Task<ActionResult> Details(string movieId)
        {
            var movie = await _movieRepository.GetAsync(movieId);
            movie.ShowTimes = movie.ShowTimes.OrderByDescending(st => st.DateTime);

            return View(movie);
        }
    }
}