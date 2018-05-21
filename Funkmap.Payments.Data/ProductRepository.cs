using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Funkmap.Payments.Core;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Parameters;

namespace Funkmap.Payments.Data
{
    internal class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _connection;

        public ProductRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> CreateAsync(Product product)
        {
            var sqlQuery = $@"INSERT INTO ""{FunkmapDataConfigurationProvider.ProductsTableName}"" (""Name"", ""Description"", ""MetaTitle"", ""MetaDescription"", ""Price"") VALUES(@Name, @MetaTitle, @MetaDescription, @Price) RETURNING ""Id""";
            var queryResult = await _connection.QueryAsync<int>(sqlQuery, product);

            var productId = queryResult.SingleOrDefault();

            product.Id = productId;

            return true;
        }

        public async Task<Product> Get(long id)
        {
            var script = $@"SELECT * FROM {FunkmapDataConfigurationProvider.ProductsTableName} WHERE ""Id"" = @Id;";
            var queryResult = await _connection.QueryAsync<Product>(script, new { Id = id });
            var product = queryResult.SingleOrDefault();
            return product;
        }

        public async Task<List<Product>> GetFilteredAsync(ProductFilter filter = null)
        {
            if (filter == null) filter = new ProductFilter();
            var script = $@"SELECT * FROM {FunkmapDataConfigurationProvider.ProductsTableName} LIMIT @Limit OFFSET @Offset;";
            var queryResults = await _connection.QueryAsync<Product>(script, new { Limit = filter.Take, Offset = filter.Skip});
            return queryResults.ToList();
        }

        public async Task<bool> IncrementSellingCountAsync(long id)
        {
            var incrementScript = $@"UPDATE {FunkmapDataConfigurationProvider.ProductsTableName} SET ""SellingCount""= ""SellingCount"" + 1 WHERE ""Id"" = @Id;";
            var result = await _connection.ExecuteAsync(incrementScript, new {Id = id});
            return result > 0;
        }
    }
}
