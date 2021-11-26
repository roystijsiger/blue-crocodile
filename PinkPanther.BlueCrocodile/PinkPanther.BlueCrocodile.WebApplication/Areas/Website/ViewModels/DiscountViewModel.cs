namespace PinkPanther.BlueCrocodile.WebApplication.Areas.Website.ViewModels
{
    public class DiscountViewModel
    {
        public OrderViewModel _orderViewModel;

        public DiscountViewModel(OrderViewModel orderViewModel)
        {
            _orderViewModel = orderViewModel;
        }

    }
}
