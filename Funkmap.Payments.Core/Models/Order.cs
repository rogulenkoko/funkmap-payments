using System;

namespace Funkmap.Payments.Core.Models
{
    public class Order
    {
        public Order()
        {
            Currency = "USD";
        }

        public long Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string CreatorLogin { get; set; }
        public User Creator { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }
       

        public decimal OrderPrice { get; set; }

        public string Currency { get; set; }
        
    }
}
