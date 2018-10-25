
namespace Funkmap.Payments.Core.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public SubscribtionPeriod Period { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool HasTrial { get; set; }
    }

    public enum PaymentType
    {
        Subscribtion = 1,
    }

    public enum SubscribtionPeriod
    {
        Monthly = 1,
        Daily = 2,
        Yearly
    }
}
