using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace Funkmap.Payments.Tests.User
{
    public class UserRepositoryTest : RepositoryTestBase
    {
        [Fact]
        public async Task CreateUserTest()
        {
            using (var unit = _unitOfWorkFactory.UnitOfWork())
            {
                var user = new Core.Models.User()
                {
                    Login = "rogulenkoko",
                    Email = "rogulenkoko@gmail.com",
                    Name = "Kirill Rogulenko"
                };

                var notCreatedUser = await unit.UserRepository.GetAsync(user.Login);
                Assert.Null(notCreatedUser);

                await unit.UserRepository.CreateAsync(user);

                var createdUser = await unit.UserRepository.GetAsync(user.Login);
                Assert.NotNull(createdUser);
                Assert.Equal(user.Login, createdUser.Login);
                Assert.Equal(user.Name, createdUser.Name);
                Assert.Equal(user.Email, createdUser.Email);
            }
        }

        [Fact]
        public async Task UpdatePaymentinfo()
        {
            var userLogin = "rogulenkoko";

            await CreateUserTest();

            using (var unit = _unitOfWorkFactory.UnitOfWork())
            {
                var paymentInfo = new TestPaymentInfo { SomeString = "some", SomeInt = 21, SomeDate = DateTime.UtcNow };
                var paymentInfoJson = JsonConvert.SerializeObject(paymentInfo);
                var result = await unit.UserRepository.UpdatePaymentInfoAsync(userLogin, paymentInfoJson);
                Assert.True(result);

                var user = await unit.UserRepository.GetAsync(userLogin);
                Assert.NotNull(user);
                Assert.Equal(userLogin, user.Login);

                var savedPaymentInfo = JsonConvert.DeserializeObject<TestPaymentInfo>(user.PaymentInfoJson);
                Assert.NotNull(savedPaymentInfo);
                Assert.Equal(savedPaymentInfo.SomeDate, paymentInfo.SomeDate);
                Assert.Equal(savedPaymentInfo.SomeInt, paymentInfo.SomeInt);
                Assert.Equal(savedPaymentInfo.SomeString, paymentInfo.SomeString);
            }
        }

        private class TestPaymentInfo
        {
            public string SomeString { get; set; }
            public int SomeInt { get; set; }
            public DateTime SomeDate { get; set; }
        }
    }
}
