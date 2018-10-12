using System.Threading.Tasks;
using Funkmap.Payments.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Funkmap.Payments.Data
{
    public class PaymentsContext : DbContext
    {
        public PaymentsContext(DbContextOptions<PaymentsContext> options) : base(options)
        {
            
        }

        public DbSet<PaymentEntity> Donations { get; set; }

        public async Task SaveAsync()
        {
            await SaveChangesAsync();
        }
    }
}
