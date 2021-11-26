using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.POS.Controllers
{
    [Area("POS")]
    [Authorize]
    public class StartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}