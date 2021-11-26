using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using PinkPanther.BlueCrocodile.WebApplication.Extensions;
using PinkPanther.BlueCrocodile.WebApplication.Middlewares;
using PinkPanther.BlueCrocodile.WebApplication.Models;

namespace PinkPanther.BlueCrocodile.WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mongoConnectionString = Configuration.GetConnectionString("MongoDb");
            var mollieKey = Configuration.GetValue<string>("MollieKey");

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddTransient(_ => HostingEnvironment);

            services.AddApplicationIdentity(mongoConnectionString);
            services.AddApplicationData(mongoConnectionString);

            services.AddTransient<IPaymentClient, PaymentClient>(_ => new PaymentClient(mollieKey));

            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            

            app.UseAuthentication();
            app.EnsureAdminExistsAsync(userManager, roleManager).Wait();

            app.UseMiddleware<ApplicationAuthenticationMiddleware>();
            

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{area:exists}/{controller=Movies}/{action=Index}/{id?}");
            });
        }
    }
}
