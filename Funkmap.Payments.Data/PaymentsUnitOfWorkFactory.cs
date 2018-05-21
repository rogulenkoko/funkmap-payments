using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Data.Abstract;

namespace Funkmap.Payments.Data
{
    public class PaymentsUnitOfWorkFactory : IPaymentsUnitOfWorkFactory
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PaymentsUnitOfWorkFactory(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IPaymentsUnitOfWork UnitOfWork()
        {
            return new PaymentsUnitOfWork(_connectionFactory);
        }
    }
}
