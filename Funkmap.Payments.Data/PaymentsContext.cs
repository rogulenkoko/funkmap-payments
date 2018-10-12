using System.Threading.Tasks;
using Funkmap.Payments.Data.Abstract;
using Funkmap.Payments.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Funkmap.Payments.Data
{
    public class PaymentsContext : DbContext, IPaymentsContext
    {
        public PaymentsContext(DbContextOptions<PaymentsContext> options) : base(options)
        {
            
        }

        public DbSet<DonationEntity> Donations { get; set; }

        public Task SaveChangesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
