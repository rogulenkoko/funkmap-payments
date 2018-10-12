using System;
using System.ComponentModel.DataAnnotations;

namespace Funkmap.Payments.Data.Entities
{
    public class DonationEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime DateUtc { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public decimal Total { get; set; }
    }
}
