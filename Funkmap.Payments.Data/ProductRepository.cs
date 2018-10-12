using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Core.Parameters;

namespace Funkmap.Payments.Data
{
    internal class ProductRepository : IProductRepository
    {

        public ProductRepository()
        {
            
        }

        public async Task<bool> CreateAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetAsync(long id)
        {
            throw new NotImplementedException();

        }

        public async Task<List<Product>> GetFilteredAsync(ProductFilter filter = null)
        {
            throw new NotImplementedException();

        }

        public async Task<bool> IncrementSellingCountAsync(long id)
        {
            throw new NotImplementedException();

        }
    }
}
