using System.Threading.Tasks;

namespace Funkmap.Payments.Core.Abstract
{
    public interface ISubscriptionEventService
    {
        Task PublishSubscriptionEventAsync(string productName, string influencedLogin);
    }
}
