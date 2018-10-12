using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Funkmap.Payments.Data.Module
{
    public static class DataModule
    {
        public static void AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PaymentsContext>(options =>
            {
                options.UseSqlServer(configuration["Database:Connection"]);
            });
        }
    }
}
