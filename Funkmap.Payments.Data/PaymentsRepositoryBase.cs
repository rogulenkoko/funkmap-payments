using System;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Data.Abstract;

namespace Funkmap.Payments.Data
{
    public abstract class PaymentsRepositoryBase : IRepositoryBase
    {
        protected readonly IPaymentsContext Context;

        protected PaymentsRepositoryBase(IPaymentsContext context)
        {
            Context = context;
        }

        public Task SaveAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}