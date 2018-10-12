using Autofac;
using Microsoft.Extensions.Configuration;
using PayPal.Abstract;

namespace PayPal
{
    public static class PayPalModule
    {
        public static void RegisterPayPalServices(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterType<PayPalService>().As<IPayPalService>().SingleInstance();
            var paypalConfigurationProvider = new PayPalConfigurationProvider(configuration);
            builder.RegisterInstance(paypalConfigurationProvider).AsSelf();
        }
    }
}
