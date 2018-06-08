using System;
using System.Data;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IPaymentsUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }

        IOrderRepository OrderRepository { get; }

        IUserRepository UserRepository { get; }

        IDbTransaction BeginTransaction();
    }
}
