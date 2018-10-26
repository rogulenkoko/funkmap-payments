using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;

namespace Funkmap.Payments.Data
{
    public abstract class PaymentsRepositoryBase
    {
        protected readonly PaymentsContext Context;

        protected PaymentsRepositoryBase(PaymentsContext context)
        {
            Context = context;
        }
    }
}