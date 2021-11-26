using Microsoft.AspNetCore.Mvc;
using PinkPanther.BlueCrocodile.Core.Models;
using PinkPanther.BlueCrocodile.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.Website.Controllers
{
    [Area("Website")]
    public class HomeController : Controller
    {

        private readonly IMovieRepository _movieRepository;

        public HomeController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet("/")]
        public async Task<ActionResult> Index()
        {
            //get all the movie times
            IEnumerable<Movie> movies = await _movieRepository.GetAllAsync();

            return View(movies);
        }
    }
}