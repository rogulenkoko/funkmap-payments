
using System.Threading.Tasks;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IDonationRepository : IRepositoryBase
    {
        Task CreateAsync(Donation order);

        void Update(Donation order);

        Task<Donation> GetAsync(long id);
    }
}
