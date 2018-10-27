using System;
using System.Threading.Tasks;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IPaymentsUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }

        IPaymentRepository PaymentRepository { get; }

        IPayPalPlanRepository PayPalPlanRepository { get; }

        ISubscriptionRepository SubscriptionRepository { get; }

        IFunkmapTransaction BeginTransaction();

        Task SaveAsync();
    }
}
