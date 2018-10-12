using System.Threading.Tasks;
using Funkmap.Payments.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Funkmap.Payments.Data.Abstract
{
    public interface IPaymentsContext
    {
        DbSet<DonationEntity> Donations { get; }

        Task SaveChangesAsync();
    }
}
