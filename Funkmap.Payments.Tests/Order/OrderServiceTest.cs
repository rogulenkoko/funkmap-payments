using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Funkmap.Auth.Client.Abstract;
using Funkmap.Auth.Contracts;
using Funkmap.Cqrs;
using Funkmap.Payments.Core;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Events;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Core.Parameters;
using Newtonsoft.Json;
using Xunit;

namespace Funkmap.Payments.Tests.Order
{
    public class OrderServiceTest : RepositoryTestBase
    {
        private readonly Auth.Contracts.User _user = new Auth.Contracts.User()
        {
            Login = "rogulenkoko",
            Name = "Kirill",
            Email = "rogulenkoko",
            Locale = "en"
        };

        [Fact]
        public async Task PositiveCreateOrder()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var paymentsService = new PositivePaymentsService();
                mock.Mock<IUserService>().Setup(x => x.GetUserAsync(_user.Login))
                    .Returns(() => Task.FromResult(new UserResponse()
                    {
                        User = _user,
                        IsExists = true
                    }));

                var userService = mock.Create<IUserService>();

                var eventBus = new InMemoryEventBus();

                eventBus.Subscribe<ProAccountConfirmedEvent>(async @event =>
                {
                    await Task.Yield();
                    Assert.Equal("rogulenkoko", @event.Login);
                });

                var orderService = new OrderService(_unitOfWorkFactory, paymentsService, new EventsFactory(), userService, eventBus);


                var proaccountProductParameter = new ProAccountParameter()
                {
                    Login = "rogulenkoko",
                    Name = "proaccount"
                };

                var order = new OrderRequest()
                {
                    Login = _user.Login
                }; 
                Assert.ThrowsAny<ArgumentException>(() => orderService.ExecuteOrderAsync(order).GetAwaiter().GetResult());

                order.PaymentParameter = new PaypalPaymentParameter();

                var result = await orderService.ExecuteOrderAsync(order);
                Assert.NotNull(result);
                Assert.True(result.Success);
            }
        }
    }

    public class PositivePaymentsService : IPaymentServiceFacade
    {
        public bool ExecutePayment<T>(Core.Models.Order order, T parameter) where T : IPaymentParameter
        {
            return true;
        }
    }

    public class NegativePaymentsService : IPaymentServiceFacade
    {
        public bool ExecutePayment<T>(Core.Models.Order order, T parameter) where T : IPaymentParameter
        {
            return false;
        }
    }
}
