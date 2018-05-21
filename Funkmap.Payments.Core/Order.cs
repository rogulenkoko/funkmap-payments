﻿using System;

namespace Funkmap.Payments.Core
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

        public User CreatedBy { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }
       

        public decimal OrderPrice { get; set; }

        public string Currency { get; set; }
        
    }
}
