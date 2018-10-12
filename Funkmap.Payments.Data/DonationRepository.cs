using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Data.Abstract;
using Funkmap.Payments.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Funkmap.Payments.Data
{
    internal class DonationMapper : PaymentsRepositoryBase, IDonationRepository
    {
        public DonationMapper(IPaymentsContext context) : base(context)
        {
        }

        public async Task CreateAsync(Donation order)
        {
            var entity = order.ToEntity();
            await Context.Donations.AddAsync(entity);
            order.Id = entity.Id;
        }

        public void Update(Donation order)
        {
            var entity = order.ToEntity();
            Context.Donations.Update(entity);
        }

        public async Task<Donation> GetAsync(long id)
        {
            var entity = await Context.Donations.SingleOrDefaultAsync(x => x.Id == id);
            return entity.ToModel();
        }
    }
}
