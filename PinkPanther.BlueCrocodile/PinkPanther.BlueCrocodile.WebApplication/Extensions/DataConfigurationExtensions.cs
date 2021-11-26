using Microsoft.Extensions.DependencyInjection;
using PinkPanther.BlueCrocodile.Core.Repositories;
using PinkPanther.BlueCrocodile.Infrastructure.Data;

namespace PinkPanther.BlueCrocodile.WebApplication.Extensions
{
    public static class DataConfigurationExtensions
    {
        public static void AddApplicationData(this IServiceCollection services, string connectionString)
        {
            var dataContext = new DataContext(connectionString);

            services.AddTransient<IDataContext, DataContext>(_ => dataContext);
            services.AddTransient<IMovieRepository, MovieRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IDiscountRepository, DiscountRepository>();
            services.AddTransient<IArrangementRepository, ArrangementRepository>();
            services.AddTransient<IRoomRepository, RoomRepository>();
            services.AddTransient<IMoviePassRepository, MoviePassRepository>();
            services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
        }
    }
}
