using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Data.Repositories;

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
            PayPalPlanRepository = new PayPalPlanRepository(context);
        }

        public IProductRepository ProductRepository { get; }
        public IPaymentRepository PaymentRepository { get; }
        public IPayPalPlanRepository PayPalPlanRepository { get; }

        public IFunkmapTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();
            return new FunkmapTransaction(transaction);
        }

        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
