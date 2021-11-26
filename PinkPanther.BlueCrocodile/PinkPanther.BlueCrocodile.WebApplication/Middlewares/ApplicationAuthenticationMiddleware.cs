using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PinkPanther.BlueCrocodile.WebApplication.Models;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.WebApplication.Middlewares
{
    public class ApplicationAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public ApplicationAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            var user = context.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                var appUser = await userManager.FindByNameAsync(user.Identity.Name);
                var userRoles = await userManager.GetRolesAsync(appUser);

                context.User = new GenericPrincipal(user.Identity, userRoles.ToArray());
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
