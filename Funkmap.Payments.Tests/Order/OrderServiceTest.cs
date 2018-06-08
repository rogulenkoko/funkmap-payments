using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Funkmap.Auth.Client;
using Funkmap.Auth.Client.Abstract;
using Funkmap.Auth.Contracts;
using Funkmap.Payments.Core;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Core.Parameters;
using Moq;
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

                var orderService = new OrderService(_unitOfWorkFactory, paymentsService, userService);


                var order = new OrderRequest()
                {
                    PaymentRequest = new PaypalRequest()
                    {
                        ProductId = 1,
                        Currency = "usd"
                    },
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
