using System.Linq;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IPaymentRepository : IRepositoryBase
    {
        Task CreateAsync(Payment payment);

        void Update(Payment payment);

        IQueryable<Payment> GetPayments();
    }
}
