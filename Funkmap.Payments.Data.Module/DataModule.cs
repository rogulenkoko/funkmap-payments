using Autofac;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Data.Repositories;
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
                options.UseNpgsql(configuration["Database:Connection"]);
                //options.UseSqlServer(configuration["Database:Connection"]);
            });
        }

        public static void RegisterDataServices(this ContainerBuilder builder)
        {
            builder.RegisterType<PaymentRepository>().As<IPaymentRepository>().InstancePerLifetimeScope();
        }
    }
}
