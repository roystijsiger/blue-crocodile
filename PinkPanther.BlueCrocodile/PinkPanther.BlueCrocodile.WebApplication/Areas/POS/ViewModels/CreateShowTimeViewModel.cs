using PinkPanther.BlueCrocodile.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.POS.ViewModels
{
    public class CreateShowTimeViewModel
    {
        public CreateShowTimeViewModel()
        {
        }

        public CreateShowTimeViewModel(string movieId, IEnumerable<Room> rooms)
        {
            MovieId = movieId;
            Rooms = rooms;
        }

        public string MovieId { get; set; }
        public IEnumerable<Room> Rooms { get; set; }

        [Required]
        public string DateTime { get; set; }

        [Required]
        public int RoomId { get; set; }
    }
}
