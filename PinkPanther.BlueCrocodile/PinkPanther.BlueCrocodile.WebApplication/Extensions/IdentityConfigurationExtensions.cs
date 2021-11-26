using AspNetCore.Identity.Mongo;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PinkPanther.BlueCrocodile.WebApplication.Models;

namespace PinkPanther.BlueCrocodile.WebApplication.Extensions
{
    public static class IdentityConfigurationExtensions
    {
        public static void AddApplicationIdentity(this IServiceCollection services, string mongoConnectionString)
        {
            services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.User.RequireUniqueEmail = true;
            }, options => { options.ConnectionString = mongoConnectionString; })
                .AddDefaultTokenProviders();
        }
    }
}
