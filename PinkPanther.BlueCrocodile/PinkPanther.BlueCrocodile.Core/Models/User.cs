using System.Collections.Generic;

namespace PinkPanther.BlueCrocodile.Core.Models
{
    /// <summary>
    ///     A user can be a customer, employee or manager.
    /// </summary>
    internal class User
    {
        public string Email { get; set; }
        public List<Order> Orders { get; set; }
    }
}