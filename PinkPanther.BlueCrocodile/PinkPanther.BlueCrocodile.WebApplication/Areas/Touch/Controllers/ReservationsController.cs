using Microsoft.AspNetCore.Mvc;
using PinkPanther.BlueCrocodile.Core.Models;
using PinkPanther.BlueCrocodile.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.Touch.Controllers
{
    [Area("Touch")]
    public class ReservationsController : Controller
    {
        private readonly IMovieRepository _movieRepository;

        public ReservationsController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<ActionResult> Reservation(string movieId, string showTimeId)
        {
            var movie = await _movieRepository.GetAsync(movieId);
            var showTime = movie.ShowTimes.FirstOrDefault(s => s.Id == showTimeId);

            if(showTime == null)
            {
                return NotFound();
            }
            
            ViewData["MovieId"] = movie.Id;
            return View(showTime);
        }

        public ActionResult CreateReservation(string movieId, string showTimeId)
        {
            // TODO: Save reservation, and pass reservationId
            return RedirectToAction(nameof(Print), new { movieId, showTimeId });
        }

        public async Task<ActionResult> Print(string movieId, string showTimeId)
        {
            // TODO: Get reservation by reservationId
            // var reservation = _reservationRepository.GetAsync(reservationId);

            var movie = await _movieRepository.GetAsync(movieId);
            var showTime = movie.ShowTimes.FirstOrDefault(s => s.Id == showTimeId);

            if(showTime == null)
            {
                return NotFound();
            }

            var reservation = new Order { 
                Movie = movie,
                ShowTime = showTime,
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        Code = Guid.NewGuid().ToString().Substring(0, 6)
                    }
                }
            };

            return View(reservation);
        }
    }
}