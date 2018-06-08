
namespace Funkmap.Payments.Core.Models
{
    public class Product
    {
        public Product()
        {
            IsDeleted = false;
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string MetaTitle { get; set; }

        public string MetaDescription { get; set; }

        public decimal Price { get; set; }

        public int SellingCount { get; set; }

        public bool IsDeleted { get; set; }
    }
}
