using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Data;
using Funkmap.Payments.Data.Repositories;
using Funkmap.Payments.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PayPal;
using Xunit;

namespace Funkmap.Payments.Tests.Payments
{
    public class PaymentsServiceTest
    {
        private readonly IPaymentsService _paymentsService;
        private readonly IProductRepository _productRepository;

        public PaymentsServiceTest()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var configurationProvider = new PayPalConfigurationProvider(configuration);
            var payPalService = new PayPalService(configurationProvider);

            var options = new DbContextOptionsBuilder<PaymentsContext>()
                .UseInMemoryDatabase("payments_service_test")
                .Options;
            var context = new PaymentsContext(options);
            DataSeeder.Seed(options);
            _productRepository = new ProductRepository(context);
            _paymentsService = new PaymentsService(payPalService, _productRepository, new PayPalPlanRepository(context));
        }


        [Fact]
        public async Task GetPayPalPlanTest()
        {
            var allProducts = await _productRepository.GetAllAsync();
            foreach (var product in allProducts)
            {
                var plan = await _paymentsService.GetOrCreatePayPalPlanIdAsync(product.Id);
                Assert.NotEqual(plan, String.Empty);
            }
        }
    }
}
