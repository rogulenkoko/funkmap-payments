using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Data.Entities
{
    public class PaymentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public DateTime DateUtc { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        [Required]
        public string ExternalId { get; set; }
    }
}
