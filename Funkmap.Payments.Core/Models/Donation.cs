using System;

namespace Funkmap.Payments.Core.Models
{
    public class Donation
    {
        public Donation()
        {
            Currency = "USD";
        }

        public long Id { get; set; }
       
        public decimal Total { get; set; }

        public string Currency { get; set; }

        public DateTime DateUtc { get; set; }

    }
}
