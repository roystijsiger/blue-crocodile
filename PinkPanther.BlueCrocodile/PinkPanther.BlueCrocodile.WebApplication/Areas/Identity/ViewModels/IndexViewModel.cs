using System.Collections.Generic;

namespace PinkPanther.BlueCrocodile.WebApplication.Areas.Identity.ViewModels
{
    public class IndexViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public List<string> Roles { get; set; }
    }
}
