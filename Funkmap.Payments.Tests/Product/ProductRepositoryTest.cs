using System;
using System.IO;
using System.Threading.Tasks;
using Funkmap.Payments.Core;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Parameters;
using Funkmap.Payments.Data;
using Funkmap.Payments.Data.Abstract;
using Funkmap.Payments.Data.Tools;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Funkmap.Payments.Tests.Product
{
    public class ProductRepositoryTest : RepositoryTestBase
    {
        [Fact]
        public async Task CreateProductTest()
        {
            using (var unitOfWork = _unitOfWorkFactory.UnitOfWork())
            {
                var product1 = new Core.Models.Product()
                {
                    Price = 100,
                    Name = "name1",
                    Description = "description1",
                    MetaDescription = "metadescription1",
                    MetaTitle = "metatitle1"
                };
                var result = await unitOfWork.ProductRepository.CreateAsync(product1);
                Assert.True(result);
                Assert.Equal(1, product1.Id);

                var product2 = new Core.Models.Product()
                {
                    Price = 100,
                    Name = "name2",
                    Description = "description1",
                    MetaDescription = "metadescription2",
                    MetaTitle = "metatitle2"
                };
                result = await unitOfWork.ProductRepository.CreateAsync(product2);
                Assert.True(result);
                Assert.Equal(2, product2.Id);


                var product3 = new Core.Models.Product()
                {
                    Price = 100,
                    Name = "name2",
                    Description = "description21",
                    MetaDescription = "description2",
                    MetaTitle = "title2"
                };

                Assert.ThrowsAny<Exception>(() => unitOfWork.ProductRepository.CreateAsync(product3).GetAwaiter().GetResult());
            }
        }

        [Fact]
        public async Task IncrementSellingCountTest()
        {
            using (var unitOfWork = _unitOfWorkFactory.UnitOfWork())
            {
                var products = await unitOfWork.ProductRepository.GetFilteredAsync();
                Assert.NotNull(products);
                Assert.NotEmpty(products);

                foreach (var product in products)
                {
                    var incrementResult = await unitOfWork.ProductRepository.IncrementSellingCountAsync(product.Id);
                    Assert.True(incrementResult);

                    var updatedProduct = await unitOfWork.ProductRepository.GetAsync(product.Id);
                    Assert.NotNull(updatedProduct);

                    Assert.Equal(updatedProduct.Name, product.Name);
                    Assert.Equal(product.SellingCount + 1, updatedProduct.SellingCount);
                }
            }
        }

        [Fact]
        public async Task GetFilteredTest()
        {
            using (var unitOfWork = _unitOfWorkFactory.UnitOfWork())
            {
                var products = await unitOfWork.ProductRepository.GetFilteredAsync();
                Assert.NotNull(products);
                Assert.NotEmpty(products);

                var defaultFilter = new ProductFilter();
                Assert.True(products.Count <= defaultFilter.Take);

                defaultFilter.Take = 1;
                products = await unitOfWork.ProductRepository.GetFilteredAsync(defaultFilter);
                Assert.NotNull(products);
                Assert.NotEmpty(products);
                Assert.Equal(1, products.Count);

                defaultFilter.Take = 3;
                products = await unitOfWork.ProductRepository.GetFilteredAsync(defaultFilter);
                Assert.NotNull(products);
                Assert.NotEmpty(products);
                Assert.Equal(2, products.Count);

                defaultFilter.Skip = 1;
                products = await unitOfWork.ProductRepository.GetFilteredAsync(defaultFilter);
                Assert.NotNull(products);
                Assert.NotEmpty(products);
                Assert.Equal(1, products.Count);

            }
        }
    }
}
