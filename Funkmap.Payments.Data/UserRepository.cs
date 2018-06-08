using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task CreateAsync(User user)
        {
            var sqlQuery = $@"INSERT INTO ""{FunkmapDataConfigurationProvider.UserTableName}"" (""Login"", ""Name"", ""Email"") VALUES(@Login, @Name, @Email)";
            await _connection.ExecuteAsync(sqlQuery, new { user.Login, user.Name, user.Email});
        }

        public async Task<User> GetAsync(string login)
        {
            var script = $@"SELECT * FROM ""{FunkmapDataConfigurationProvider.UserTableName}"" WHERE ""Login"" = @Login;";
            var queryResult = await _connection.QueryAsync<User>(script, new { Login = login });
            var user = queryResult.SingleOrDefault();
            return user;
        }

        public async Task<bool> UpdatePaymentInfoAsync(string login, string paymentInfoJson)
        {
            var incrementScript = $@"UPDATE ""{FunkmapDataConfigurationProvider.UserTableName}"" SET ""PaymentInfoJson"" = @PaymentInfoJson WHERE ""Login"" = @Login;";
            var result = await _connection.ExecuteAsync(incrementScript, new { PaymentInfoJson = paymentInfoJson, Login = login });
            return result > 0;
        }
    }
}
