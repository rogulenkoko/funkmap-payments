using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Data.Entities
{
    public class SubscriptionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string InfluencedLogin { get; set; }

        public string ProductName { get; set; }
        
        public ProductEntity Product { get; set; }

        public SubscriptionStatus Status { get; set; }

        public decimal PricePerPeriod { get; set; }

        public string Currency { get; set; }

        public string PayPalAgreementId { get; set; }
    }
}
