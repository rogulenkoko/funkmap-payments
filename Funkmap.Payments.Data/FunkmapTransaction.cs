using Funkmap.Payments.Core.Abstract;
using Microsoft.EntityFrameworkCore.Storage;

namespace Funkmap.Payments.Data
{
    public class FunkmapTransaction : IFunkmapTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public FunkmapTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }
        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }
    }
}
