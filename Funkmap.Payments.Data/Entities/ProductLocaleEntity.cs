using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Funkmap.Payments.Data.Entities
{
    public class ProductLocaleEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Language { get; set; }
        
        [Required]
        public string ProductName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Currency { get; set; }

        public decimal Total { get; set; }
    }
}