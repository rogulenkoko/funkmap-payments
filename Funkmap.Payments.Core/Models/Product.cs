﻿
namespace Funkmap.Payments.Core.Models
{
    /// <summary>
    /// Product model
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unique product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Translated product name
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Translated product description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Price of one period item
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Subscription period (Month, Day, Year)
        /// </summary>
        public SubscriptionPeriod Period { get; set; }

        /// <summary>
        /// Payment type (subscription)
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// Has trial period or not
        /// </summary>
        public bool HasTrial { get; set; }
    }

    public enum PaymentType
    {
        /// <summary>
        /// Subscription
        /// </summary>
        Subscription = 1,
    }

    public enum SubscriptionPeriod
    {
        Monthly = 1,
        Daily = 2,
        Yearly
    }
}
