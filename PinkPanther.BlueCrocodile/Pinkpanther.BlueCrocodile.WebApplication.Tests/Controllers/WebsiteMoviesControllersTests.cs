using Microsoft.AspNetCore.Mvc;
using PinkPanther.BlueCrocodile.Core.Repositories;
using PinkPanther.BlueCrocodile.WebApplication.Areas.Website.Controllers;
using PinkPanther.BlueCrocodile.WebApplication.Tests.Mocks;
using PinkPanther.BlueCrocodile.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using Xunit;

namespace PinkPanther.BlueCrocodile.WebApplication.Tests.Controllers
{
    public class WebsiteMovieControllerTests
    {
        private MoviesController _controller;
        private IMovieRepository _repository;

        public WebsiteMovieControllerTests()
        {
            _repository = new MockMovieRepository();
        }

        [Fact]
        public async void NewMoviesController_GetMovies_ByFilter_Upcomming7Days()
        {
            //arrange
            _controller = new MoviesController(_repository);

            //act
            var result = await _controller.ByWeek();

            //asert
            Assert.NotNull(result);

            Assert.IsAssignableFrom<ViewResult>(result);
            var viewResult = result as ViewResult;

            Assert.IsAssignableFrom<IEnumerable<MovieDetailsShowTimeView>>(viewResult.Model);
            var movies = viewResult.Model as IEnumerable<MovieDetailsShowTimeView>;
            Assert.NotNull(movies);
            

            var movieList = movies as List<MovieDetailsShowTimeView>;

            Assert.Single(movieList);
            Assert.Equal("Movie 1", movieList[0].Title);
        }

        [Fact]
        public async void NewMoviesController_GetMovies_ByFilter_Between8DaysAnd14DaysFromNow()
        {
            //arrange
            _controller = new MoviesController(_repository);

            //act
            var result = await _controller.ByWeek(0, DateTime.Now.AddDays(8), DateTime.Now.AddDays(14));

            //asert
            Assert.NotNull(result);

            Assert.IsAssignableFrom<ViewResult>(result);
            var viewResult = result as ViewResult;

            Assert.IsAssignableFrom<IEnumerable<MovieDetailsShowTimeView>>(viewResult.Model);
            var movies = viewResult.Model as IEnumerable<MovieDetailsShowTimeView>;
            Assert.NotNull(movies);
            

            var movieList = movies as List<MovieDetailsShowTimeView>;

            Assert.Single(movieList);
            Assert.Equal("John Wick II", movieList[0].Title);
        }

        [Fact]
        public async void NewMoviesController_GetMovies_ByFilter_Room5()
        {
            //arrange
            _controller = new MoviesController(_repository);

            //act
            var result = await _controller.ByWeek(5, DateTime.MinValue, DateTime.MinValue);

            //asert
            Assert.NotNull(result);

            Assert.IsAssignableFrom<ViewResult>(result);
            var viewResult = result as ViewResult;

            Assert.IsAssignableFrom<IEnumerable<MovieDetailsShowTimeView>>(viewResult.Model);
            var movies = viewResult.Model as IEnumerable<MovieDetailsShowTimeView>;
            Assert.NotNull(movies);


            var movieList = movies as List<MovieDetailsShowTimeView>;

            Assert.Single(movieList);
            Assert.Equal("John Wick II", movieList[0].Title);
        }
    }
}
