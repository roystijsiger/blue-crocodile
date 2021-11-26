using Microsoft.AspNetCore.Mvc;
using PinkPanther.BlueCrocodile.Core.Models;
using PinkPanther.BlueCrocodile.Core.Repositories;
using PinkPanther.BlueCrocodile.WebApplication.Areas.POS.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.POS.Controllers
{
    [Area("POS")]
    [Route("[controller]/[action]")]
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IRoomRepository _roomRepository;

        public MoviesController(IMovieRepository movieRepository, IRoomRepository roomRepository)
        {
            _movieRepository = movieRepository;
            _roomRepository = roomRepository;
        }

        public async Task<IActionResult> Overview()
        {
            var movies = await _movieRepository.GetAllAsync();
            var model = movies.OrderBy(m => m.Title).ToList();

            return View(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Manage(string id)
        {
            var movie = await _movieRepository.GetAsync(id);
            var showTimes = movie.ShowTimes;

            movie.ShowTimes = showTimes.OrderBy(o => o.DateTime).ToList();

            return View(movie);
        }

        [HttpGet]
        public async Task<IActionResult> CreateShowtime(string movieId)
        {
            var movie = _movieRepository.GetAsync(movieId);
            if (movie == null)
            {
                return NotFound();
            }

            var rooms = await _roomRepository.GetAllAsync();

            return View(new CreateShowTimeViewModel(movieId, rooms));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShowtime(CreateShowTimeViewModel model)
        {
            if (!ModelState.IsValid) {
                model.Rooms = await _roomRepository.GetAllAsync();
                return View();
            }

            var movie = await _movieRepository.GetAsync(model.MovieId);
            if (movie == null)
            {
                ModelState.AddModelError("MovieNotFound", "Could not find movie for new showtime.");
                model.Rooms = await _roomRepository.GetAllAsync();
                return View();
            }

            var room = await _roomRepository.GetAsync(model.RoomId.ToString());

            if(!DateTime.TryParse(model.DateTime, new CultureInfo("en-US"), DateTimeStyles.AllowWhiteSpaces, out var datetime))
            {
                ModelState.AddModelError("InvalidDateTime", "The provided datetime is invalid.");
                model.Rooms = await _roomRepository.GetAllAsync();
                return View();
            }

            var showtime = new ShowTime
            {
                Room = room,
                DateTime = datetime
            };

            var showtimes = movie.ShowTimes.ToList();
            showtimes.Add(showtime);

            movie.ShowTimes = showtimes;

            await _movieRepository.UpdateAsync(movie);

            return RedirectToAction("Manage", "Movies", new { id = movie.Id });
        }



        [HttpGet]
        public async Task<IActionResult> DeleteShowtime(string movieId, string showTimeId)
        {
            var movie = await _movieRepository.GetAsync(movieId);
            var showtimes = movie.ShowTimes.ToList();

            var showtime = showtimes.SingleOrDefault(s => s.Id == showTimeId);
            if (showtime == null) return RedirectToAction(nameof(Manage), new { movieId });

            showtimes.Remove(showtime);

            movie.ShowTimes = showtimes;

            await _movieRepository.UpdateAsync(movie);

            return RedirectToAction("Manage", "Movies", new { id = movieId });
        }
    }
}