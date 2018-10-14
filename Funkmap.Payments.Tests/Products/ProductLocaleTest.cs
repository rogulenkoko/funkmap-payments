using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Data;
using Funkmap.Payments.Data.Repositories;
using Funkmap.Payments.Data.Tools;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Funkmap.Payments.Tests.Products
{
    public class ProductLocaleTest
    {
        private readonly IProductRepository _productRepository;
        public ProductLocaleTest()
        {
            var options = new DbContextOptionsBuilder<PaymentsContext>()
                .UseInMemoryDatabase("products_locale_test")
                .Options;
            Seed(options);
            var context = new PaymentsContext(options);
            context.Database.EnsureCreated();
            _productRepository = new ProductRepository(context);
        }

        [Fact]
        public async Task GetAllProductsTest()
        {
            //default
            var allProducts = await _productRepository.GetAllAsync();
            Assert.NotEmpty(allProducts);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var allProductsEn = await _productRepository.GetAllAsync();
            Assert.NotEmpty(allProducts);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            var allProductsRu = await _productRepository.GetAllAsync();
            Assert.NotEmpty(allProducts);

            foreach (var product in allProducts)
            {
                var productEn = allProductsEn.Single(x => x.Id == product.Id);
                var productRu = allProductsRu.Single(x => x.Id == product.Id);

                Assert.Equal(product.Name, productEn.Name);
                Assert.Equal(product.Description, productEn.Description);
                Assert.Equal(product.Currency, productEn.Currency);
                Assert.Equal(product.Price, productEn.Price);

                Assert.NotEqual(productRu.Name, String.Empty);
                Assert.NotEqual(product.Name, productRu.Name);

                Assert.NotEqual(productRu.Description, String.Empty);
                Assert.NotEqual(product.Description, productRu.Description);

                Assert.NotEqual(productRu.Currency, String.Empty);
                Assert.NotEqual(product.Currency, productRu.Currency);
            }
        }

        private void Seed(DbContextOptions<PaymentsContext> options)
        {
            using (var context = new PaymentsContext(options))
            {
                context.ProductLocales.AddRange(PaymentsStaticDatProdiver.ProductLocales);
                context.Products.AddRange(PaymentsStaticDatProdiver.Products);
                context.SaveChanges();
            }
        }
    }
}
