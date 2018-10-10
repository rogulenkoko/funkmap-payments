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
        private readonly PayPalConfigurationProvider _configurationProvider;

        public PayPalTest()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            _configurationProvider = new PayPalConfigurationProvider(configuration);
        }

        [Fact]
        public async Task CreatePayment()
        {
            IPayPalService payPal = new PayPalService(_configurationProvider);

            var payment = new PayPalPayment()
            {
                Currency = "USD",
                ReturnUrl = "http://localhost:1230",
                CancelUrl = "http://localhost:1230",
                Total = 0.02m
            };
            var response = await payPal.CreatePaymentAsync(payment);
        }
    }
}
