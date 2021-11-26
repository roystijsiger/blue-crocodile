using PinkPanther.BlueCrocodile.WebApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.Identity.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }

        // Requirements are handled by user manager
        [Required]
        public string Password { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
