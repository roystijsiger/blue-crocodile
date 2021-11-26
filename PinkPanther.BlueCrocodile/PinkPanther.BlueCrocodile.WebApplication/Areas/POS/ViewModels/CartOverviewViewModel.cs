using PinkPanther.BlueCrocodile.WebApplication.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.POS.ViewModels
{
    public class CartOverviewViewModel
    {
        public CartOverviewViewModel()
        {
        }

        public Cart Cart { get; set; }

        public IEnumerable<IGrouping<string, MovieShowTimeViewModel>> Showtimes { get; set; }

        [Required]
        public string Action { get; set; }

        public string SelectedShowTimeId { get; set; }
        public string SelectedSubscription { get; set; }
    }

    public enum CartOverviewAction
    {
        Unknown,
        AddReservation,
        AddSubscription,
        AddMoviePass
    }
}
