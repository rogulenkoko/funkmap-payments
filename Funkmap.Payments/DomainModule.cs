using Autofac;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Services;

namespace Funkmap.Payments
{
    public static class DomainModule
    {
        public static void RegisterDomainServices(this ContainerBuilder builder)
        {
            builder.RegisterType<PaymentsService>().As<IPaymentsService>().InstancePerLifetimeScope();
        }
    }
}
