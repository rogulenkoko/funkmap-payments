using System.Data;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Data.Abstract;

namespace Funkmap.Payments.Data
{
    public class PaymentsUnitOfWork : IPaymentsUnitOfWork
    {
        private readonly IDbConnection _dbConnection;

        private IProductRepository _productRepository;

        public PaymentsUnitOfWork(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnection = dbConnectionFactory.Connection();
        }

        public IProductRepository ProductRepository => _productRepository ?? (_productRepository = new ProductRepository(_dbConnection));

        public void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }
}
