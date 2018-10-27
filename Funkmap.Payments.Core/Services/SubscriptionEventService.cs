using System;
using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Payments.Contracts.Events;
using Funkmap.Payments.Core.Abstract;

namespace Funkmap.Payments.Core.Services
{
    public class SubscriptionEventService : ISubscriptionEventService
    {
        private readonly IEventBus _eventBus;

        public SubscriptionEventService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task PublishSubscriptionEventAsync(string productName, string influencedLogin)
        {
            object @event;
            switch (productName)
            {
                case FunkmapPaymentConstants.ProAccount:
                    @event = new ProAccountConfirmedEvent(influencedLogin);
                    break;

                case FunkmapPaymentConstants.PriorityProfile:
                    @event = new PriorityMarkerConfirmedEvent(influencedLogin);
                    break;

                default: throw new InvalidOperationException("Invalid product name.");
            }

            await _eventBus.PublishAsync(@event);
        }
    }
}
