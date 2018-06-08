using System;
using System.Data;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Data.Abstract;

namespace Funkmap.Payments.Data
{
    public class PaymentsUnitOfWork : IPaymentsUnitOfWork
    {
        private readonly IDbConnection _dbConnection;

        private IProductRepository _productRepository;
        private IUserRepository _userRepository;
        private IOrderRepository _orderRepository;

        public PaymentsUnitOfWork(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnection = dbConnectionFactory.Connection();
            _dbConnection.Open();
        }

        public IProductRepository ProductRepository => _productRepository ?? (_productRepository = new ProductRepository(_dbConnection));
        public IOrderRepository OrderRepository => _orderRepository ?? (_orderRepository = new OrderRepository(_dbConnection));
        public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(_dbConnection));


        public IDbTransaction BeginTransaction()
        {
            return _dbConnection.BeginTransaction();
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }
}
