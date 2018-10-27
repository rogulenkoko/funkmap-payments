
namespace Funkmap.Payments.Models
{
    /// <summary>
    /// Subscription request
    /// </summary>
    public class SubscriptionRequest
    {
        /// <summary>
        /// Unique product name
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Influenced user or profile login
        /// </summary>
        public string Login { get; set; }
    }
}
