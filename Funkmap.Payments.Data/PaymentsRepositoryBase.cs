using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;

namespace Funkmap.Payments.Data
{
    public abstract class PaymentsRepositoryBase : IRepositoryBase
    {
        protected readonly PaymentsContext Context;

        protected PaymentsRepositoryBase(PaymentsContext context)
        {
            Context = context;
        }

        public Task SaveAsync()
        {
            return Context.SaveAsync();
        }
    }
}