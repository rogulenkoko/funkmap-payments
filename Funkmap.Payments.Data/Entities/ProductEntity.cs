using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Data.Entities
{
    public class ProductEntity
    {
        [Key]
        public string Name { get; set; }
        public SubscribtionPeriod Period { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool HasTrial { get; set; }
        public virtual List<ProductLocaleEntity> ProductLocales { get; set; }
    }
}