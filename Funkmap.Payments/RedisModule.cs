using Autofac;
using Funkmap.Payments.Core;
using StackExchange.Redis;

namespace Funkmap.Payments
{
    public static class RedisModule
    {
        public static void RegisterRedisServices(this ContainerBuilder builder)
        {
            builder.Register(container =>
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(FunkmapConfigurationProvider.Configuration["Redis:Connection"]);
                return redis;
            }).As<ConnectionMultiplexer>().SingleInstance().OnRelease(x => x.Dispose());

            builder.Register(container =>
                {
                    IDatabase db = container.Resolve<ConnectionMultiplexer>().GetDatabase(1, asyncState: true);
                    return db;
                })
                .As<IDatabase>()
                .SingleInstance();


            builder.Register(container =>
                {
                    ISubscriber sub = container.Resolve<ConnectionMultiplexer>().GetSubscriber();
                    return sub;
                })
                .As<ISubscriber>()
                .SingleInstance();
        }
    }
}
