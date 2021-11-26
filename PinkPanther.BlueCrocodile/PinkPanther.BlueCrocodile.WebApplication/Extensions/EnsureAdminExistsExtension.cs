using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using PinkPanther.BlueCrocodile.WebApplication.Models;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.WebApplication.Extensions
{
    public static class EnsureAdminExistsExtension
    {
        public static async Task EnsureAdminExistsAsync(this IApplicationBuilder app, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            const string email = "admin@example.com";
            const string password = "tSEXjp3!HHka!7UhKcgE2p6dXhGku@tz";

            var originalUser = await userManager.FindByNameAsync(email);
            if (originalUser != null)
            {
                await userManager.DeleteAsync(originalUser);
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            await userManager.CreateAsync(user, password);

            if (!await roleManager.RoleExistsAsync(nameof(Role.Administrator)))
            {
                await roleManager.CreateAsync(new ApplicationRole(nameof(Role.Administrator)));
            }

            await userManager.AddToRoleAsync(user, nameof(Role.Administrator));
        }
    }
}
