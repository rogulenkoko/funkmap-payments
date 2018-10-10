
namespace Funkmap.Payments.Models
{
    public class DonationRequest
    {
        public Currency Currency { get; set; }

        public decimal Total { get; set; }
    }

    public enum Currency
    {
        USD = 1,
        RUB = 2
    }
}
