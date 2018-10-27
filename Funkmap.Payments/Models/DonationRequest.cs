
namespace Funkmap.Payments.Models
{
    /// <summary>
    /// Donation request
    /// </summary>
    public class DonationRequest
    {
        /// <summary>
        /// Currency (USD, RUB)
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Price value
        /// </summary>
        public decimal Total { get; set; }
    }

    public enum Currency
    {
        /// <summary>
        /// US Dollars
        /// </summary>
        USD = 1,

        /// <summary>
        /// Russian Rubles
        /// </summary>
        RUB = 2
    }
}
