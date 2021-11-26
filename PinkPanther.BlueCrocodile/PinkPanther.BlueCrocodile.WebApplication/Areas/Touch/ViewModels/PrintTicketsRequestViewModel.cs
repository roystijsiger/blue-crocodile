using System.ComponentModel.DataAnnotations;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.Touch.ViewModels
{
    public class PrintTicketsRequestViewModel
    {
        [Required]
        public string Code { get; set; }
    }
}
