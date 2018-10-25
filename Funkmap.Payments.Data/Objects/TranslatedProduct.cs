using Funkmap.Payments.Data.Entities;

namespace Funkmap.Payments.Data.Objects
{
    public class TranslatedProduct
    {
        public ProductEntity Product { get; set; }

        public ProductLocaleEntity Locale { get; set; }
    }
}
