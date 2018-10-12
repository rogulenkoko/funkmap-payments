using System.Linq;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Data.Mappers;

namespace Funkmap.Payments.Data
{
    public class PaymentRepository : PaymentsRepositoryBase, IPaymentRepository
    {
        public PaymentRepository(PaymentsContext context) : base(context)
        {
        }

        public async Task CreateAsync(Payment payment)
        {
            var entity = payment.ToEntity();
            await Context.Donations.AddAsync(entity);
            payment.Id = entity.Id;
        }

        public void Update(Payment payment)
        {
            var entity = payment.ToEntity();
            Context.Donations.Update(entity);
        }

        public IQueryable<Payment> GetPayments()
        {
            return Context.Donations.Select(x => x.ToModel());
        }
    }
}
