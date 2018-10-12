using System;

namespace Funkmap.Payments.Core.Models
{
    public class Payment
    {
        public Payment()
        {
            Currency = "USD";
        }

        public long Id { get; set; }
       
        public decimal Total { get; set; }

        public string Currency { get; set; }

        public DateTime DateUtc { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public string ExternalId { get; set; }

    }

    public enum PaymentStatus
    {
        Created = 1,
        Executed = 2,
        Canceled = 3
    }
}
