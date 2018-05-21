using System.Data;
using Funkmap.Payments.Data.Abstract;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Funkmap.Payments.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection Connection()
        {
            var connectionString = _configuration["Database:Connection"];
            var connection = new NpgsqlConnection(connectionString);
            return connection;
        }
    }
}
