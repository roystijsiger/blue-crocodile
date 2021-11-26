using System.ComponentModel;

namespace PinkPanther.BlueCrocodile.WebApplication.Models
{
    public enum Role
    {
        [DisplayName(nameof(Administrator))]
        Administrator, 

        [DisplayName(nameof(Manager))]
        Manager,

        [DisplayName(nameof(Employee))]
        Employee,

        [DisplayName(nameof(Customer))]
        Customer
    }

    public static class RoleExtensions
    {
        public static string ToString(this Role role) => role switch
        {
            Role.Administrator => nameof(Role.Administrator),
            Role.Manager => nameof(Role.Manager),
            Role.Employee => nameof(Role.Employee),
            Role.Customer => nameof(Role.Customer),

            _ => throw new InvalidEnumArgumentException(nameof(role))
        };

    }
}
