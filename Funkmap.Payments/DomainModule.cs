using Autofac;
using Funkmap.Auth.Client;
using Funkmap.Auth.Client.Abstract;
using Funkmap.Cqrs;
using Funkmap.Cqrs.Abstract;
using Funkmap.Payments.Core;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Services;
using Funkmap.Payments.Services;
using Funkmap.Redis;

namespace Funkmap.Payments
{
    public static class DomainModule
    {
        public static void RegisterDomainServices(this ContainerBuilder builder)
        {
            builder.RegisterType<PaymentsService>().As<IPaymentsService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductValidationService>().As<IProductValidationService>().InstancePerLifetimeScope();
            builder.Register(context =>
            {
                var url = FunkmapConfigurationProvider.Configuration["Bandmap:Url"];
                return new UserService(url);
            }).As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<SubscriptionEventService>().As<ISubscriptionEventService>().InstancePerLifetimeScope();
            //builder.RegisterType<RedisEventBus>().As<IEventBus>();
            builder.RegisterType<InMemoryEventBus>().As<IEventBus>();
        }
    }
}
