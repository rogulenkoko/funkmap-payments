using System.ComponentModel.DataAnnotations;

namespace Funkmap.Payments.Data.Entities
{
    public class PayPalPlanEntity
    {
        public string Id { get; set; }

        [Required]
        public long ProductLocaleId { get; set; }
    }
}
