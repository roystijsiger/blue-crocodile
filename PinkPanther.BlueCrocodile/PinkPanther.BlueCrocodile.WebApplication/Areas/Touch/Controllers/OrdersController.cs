using Microsoft.AspNetCore.Mvc;
using PinkPanther.BlueCrocodile.Core.Repositories;
using PinkPanther.BlueCrocodile.WebApplication.Areas.Touch.ViewModels;
using PinkPanther.BlueCrocodile.WebApplication.Areas.Website.ViewModels;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.Touch.Controllers
{
    [Area("Touch")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public ActionResult PrintTicketsRequest()
        {
            return View();
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PrintTicketsRequest(PrintTicketsRequestViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var order = await _orderRepository.GetByCodeAsync(viewModel.Code);
            if (order == null)
            {
                ModelState.AddModelError("code", "The provided code is invalid.");
                return View(viewModel);
            }

            var vm = new OrderViewModel(order, _orderRepository);

            return View("PrintTickets", vm);
        }
        
    }
}
