using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnection _connection;

        public OrderRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> CreateAsync(Order order)
        {
            var sqlQuery = $@"INSERT INTO ""{FunkmapDataConfigurationProvider.OrderTableName}"" (""CreatedOn"", ""UpdatedOn"", ""CreatorLogin"", ""ProductId"", ""OrderPrice"", ""Currency"") VALUES(@CreatedOn, @UpdatedOn, @CreatorLogin, @ProductId, @OrderPrice, @Currency) RETURNING ""Id""";
            var queryResult = await _connection.QueryAsync<int>(sqlQuery, new { order.CreatedOn, order.UpdatedOn, order.CreatorLogin, order.ProductId, order.OrderPrice, order.Currency });

            var orderId = queryResult.SingleOrDefault();

            order.Id = orderId;

            return true;
        }

        public async Task<Order> GetAsync(long id)
        {
            var script = @"SELECT * FROM ""public.order_test"" as ""order""
                    INNER JOIN ""public.user_test"" as ""user"" on ""user"".""Login"" = ""order"".""CreatorLogin""
                    INNER JOIN ""public.product_test"" as ""product"" on ""product"".""Id"" = ""order"".""ProductId"" 
                    WHERE ""order"".""Id"" = @Id;";

            //var script = $@"SELECT * FROM ""{FunkmapDataConfigurationProvider.OrderTableName}"" WHERE ""Id"" = @Id;";
            var queryResult = await _connection.QueryAsync<Order, User, Product, Order>(script, (o, user, product) =>
            {
                o.Creator = user;
                o.CreatorLogin = user.Login;

                o.Product = product;
                o.ProductId = product.Id;
                return o;
            }, new {Id = id}, splitOn: "Login,Id");
            var order = queryResult.SingleOrDefault();
            return order;
        }
    }
}
