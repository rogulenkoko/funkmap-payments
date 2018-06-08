using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Core.Parameters;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IProductRepository
    {
        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<bool> CreateAsync(Product product);

        /// <summary>
        /// Increment product selling count
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns></returns>
        Task<bool> IncrementSellingCountAsync(long id);

        /// <summary>
        /// Get product by Id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns></returns>
        Task<Product> GetAsync(long id);

        /// <summary>
        /// Get filtered products
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<List<Product>> GetFilteredAsync(ProductFilter filter = null);
    }
}
