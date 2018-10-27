using System.Linq;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Core.Abstract
{
    public interface ISubscriptionRepository
    {
        IQueryable<Subscription> GetSubscriptions();

        Task CreateAsync(Subscription subscription);

        void Update(Subscription subscription);
    }
}
