using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PayPal;
using PayPal.Abstract;
using PayPal.Contracts;
using Xunit;

namespace Funkmap.Payments.Tests.PayPal
{
    public class PayPalTest
    { 
        private readonly IPayPalService _payPalService;

        public PayPalTest()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var configurationProvider = new PayPalConfigurationProvider(configuration);
            _payPalService = new PayPalService(configurationProvider);
        }

        [Fact]
        public async Task CreatePayment()
        {
            var payment = new PayPalPayment()
            {
                Currency = "USD",
                ReturnUrl = "http://localhost:1230",
                CancelUrl = "http://localhost:1230",
                Total = 0.02m
            };
            var response = await _payPalService.CreatePaymentAsync(payment);

            Assert.NotNull(response);
            Assert.NotEqual(response.Id, String.Empty);
            Assert.NotNull(response.Id);
            Assert.NotEqual(response.RedirectUrl, String.Empty);
            Assert.NotNull(response.RedirectUrl);
        }

        [Fact]
        public async Task CreatePlan()
        {
            var plan = new PayPalPlan
            {
                Currency = "RUB",
                Total = 100.00m,
                Name = "pro_account",
                PeriodType = PeriodType.Month,
                Description = "Pro account description",
                Frequency = 1
            };

            await _payPalService.CreatePlanAsync(plan);
        }
    }
}
