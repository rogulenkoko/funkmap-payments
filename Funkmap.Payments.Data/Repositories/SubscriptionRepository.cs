using System.Linq;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Data.Mappers;

namespace Funkmap.Payments.Data.Repositories
{
    public class SubscriptionRepository : PaymentsRepositoryBase, ISubscriptionRepository
    {
        public SubscriptionRepository(PaymentsContext context) : base(context)
        {
        }

        public IQueryable<Subscription> GetSubscriptions()
        {
            return Context.Subscriptions.Select(x => x.ToModel());
        }

        public async Task CreateAsync(Subscription subscription)
        {
            var entity = subscription.ToEntity();
            await Context.Subscriptions.AddAsync(entity);
        }

        public void Update(Subscription subscription)
        {
            var entity = subscription.ToEntity();
            Context.Subscriptions.Update(entity);
        }
    }
}
