using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Data.Entities;

namespace Funkmap.Payments.Data.Mappers
{
    public static class SubscriptionMapper
    {
        public static SubscriptionEntity ToEntity(this Subscription source)
        {
            if (source == null) return null;
            return new SubscriptionEntity()
            {
                Id = source.Id,
                ProductName = source.ProductName,
                InfluencedLogin = source.InfluencedLogin,
                Status = source.Status,
                Currency = source.Currency,
                PricePerPeriod = source.PricePerPeriod,
                PayPalAgreementId = source.PayPalAgreementId
            };
        }

        public static Subscription ToModel(this SubscriptionEntity source)
        {
            if (source == null) return null;
            return new Subscription()
            {
                Id = source.Id,
                ProductName = source.ProductName,
                InfluencedLogin = source.InfluencedLogin,
                Status = source.Status,
                Currency = source.Currency,
                PricePerPeriod = source.PricePerPeriod,
                PayPalAgreementId = source.PayPalAgreementId
            };
        }
    }
}
