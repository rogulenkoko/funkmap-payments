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
        public async Task CreateSubscription()
        {
            var plan = new PayPalPlan
            {
                Currency = "RUB",
                Total = 100m,
                Name = "Basic annual subscription",
                PeriodType = PeriodType.Month,
                Description = "Annual subscription plan (Russia 100r.)",
                Frequency = 1
            };

            await _payPalService.CreatePlanAsync(plan);
            await _payPalService.ActivatePlanAsync(plan);


            var agreement = new PayPalAgreement
            {
                Name = "Bandmap pro-account",
                Description = "Annual subscription giving special features to the user on the Bandmap service",
                PayPalPlanId = plan.Id
            };
            var agreementResult = await _payPalService.CreateAgreementAsync(agreement);
        }

        [Fact]
        public async Task ExecuteAgreement()
        {
            var token = "EC-77U794285N940942N";
            await _payPalService.ExecuteAgreementAsync(token);
        }
    }
}
