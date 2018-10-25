using System;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IPaymentsUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }

        IPaymentRepository PaymentRepository { get; }

        IFunkmapTransaction BeginTransaction();
    }
}
