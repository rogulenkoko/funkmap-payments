using System;
using System.Threading.Tasks;
using Npgsql;
using Xunit;

namespace Funkmap.Payments.Tests.Order
{
    public class OrderRepositoryTest : RepositoryTestBase
    {
        [Fact]
        public async Task CreateOrderTest()
        {
            using (var unitOfWork = _unitOfWorkFactory.UnitOfWork())
            {
                var order = new Core.Models.Order()
                {
                    CreatedOn = DateTime.UtcNow,
                    CreatorLogin = "rogulenkoko",
                    Currency = "usd",
                    OrderPrice = 12.21m,
                    ProductId = 1,
                    UpdatedOn = DateTime.UtcNow
                };

                //there is no referenced product and user
                Assert.ThrowsAny<PostgresException>(() => unitOfWork.OrderRepository.CreateAsync(order).GetAwaiter().GetResult());

                var user = new Core.Models.User()
                {
                    Login = "rogulenkoko",
                    Email = "rogulenkoko@gmail.com",      
                    Name = "Kirill Rogulenko"
                };

                await unitOfWork.UserRepository.CreateAsync(user);

                //there is no referenced product
                Assert.ThrowsAny<PostgresException>(() => unitOfWork.OrderRepository.CreateAsync(order).GetAwaiter().GetResult());

                var product = new Core.Models.Product()
                {
                    Name = "test",
                    Description = "test",
                    MetaDescription = "test",
                    MetaTitle = "test",
                    Price = 12
                };

                var result = await unitOfWork.ProductRepository.CreateAsync(product);
                Assert.True(result);

                order.ProductId = product.Id;
                await unitOfWork.OrderRepository.CreateAsync(order);
                Assert.NotEqual(0, order.Id);

                var savedOrder = await unitOfWork.OrderRepository.GetAsync(order.Id);
                Assert.NotNull(savedOrder);
                Assert.NotNull(savedOrder.Product);
                Assert.NotNull(savedOrder.Creator);

                Assert.Equal(order.OrderPrice, savedOrder.OrderPrice);
                Assert.Equal(order.UpdatedOn.ToString(), savedOrder.UpdatedOn.ToString());
                Assert.Equal(order.CreatedOn.ToString(), savedOrder.CreatedOn.ToString());
                Assert.Equal(order.ProductId, savedOrder.ProductId);
                Assert.Equal(order.ProductId, savedOrder.Product.Id);
                Assert.Equal(order.CreatorLogin, savedOrder.Creator.Login);
                Assert.Equal(order.Currency, savedOrder.Currency);
                
                Assert.Equal(user.Email, savedOrder.Creator.Email);
                Assert.Equal(user.Name, savedOrder.Creator.Name);
                Assert.Equal(user.Login, savedOrder.Creator.Login);

                Assert.Equal(product.Name, savedOrder.Product.Name);
                Assert.Equal(product.Description, savedOrder.Product.Description);
                Assert.Equal(product.Id, savedOrder.Product.Id);
                Assert.Equal(product.MetaDescription, savedOrder.Product.MetaDescription);
                Assert.Equal(product.MetaTitle, savedOrder.Product.MetaTitle);
                Assert.Equal(product.Price, savedOrder.Product.Price);
            }
        }
    }
}
