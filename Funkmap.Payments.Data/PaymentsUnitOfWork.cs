using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Funkmap.Payments.Data
{
    public class PaymentsUnitOfWork : IPaymentsUnitOfWork
    {
        private readonly PaymentsContext _context;

        public PaymentsUnitOfWork(PaymentsContext context)
        {
            _context = context;
            ProductRepository = new ProductRepository(context);
            PaymentRepository = new PaymentRepository(context);
        }

        public IProductRepository ProductRepository { get; }
        public IPaymentRepository PaymentRepository { get; }
        public IFunkmapTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();
            return new FunkmapTransaction(transaction);
        }
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
