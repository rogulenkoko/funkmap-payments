namespace Funkmap.Payments.Core.Models
{
    public class Subscription
    {
        public long Id { get; set; }

        public string InfluencedLogin { get; set; }

        public string ProductName { get; set; }

        public SubscriptionStatus Status { get; set; }

        public decimal PricePerPeriod { get; set; }

        public string Currency { get; set; }

        public string PayPalAgreementId { get; set; }
    }

    public enum SubscriptionStatus
    {
        Pending = 1,
        Active = 2,
        Canceled = 3
    }
}
