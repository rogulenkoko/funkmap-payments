using System.Linq;
using System.Threading;
using Funkmap.Payments.Data.Entities;
using Funkmap.Payments.Data.Objects;

namespace Funkmap.Payments.Data.Tools
{
    public static class ProductsQueryProvider
    {
        private const string DeafultCulture = "en-US";

        public static IQueryable<TranslatedProduct> GetTranslatedProducts(this IQueryable<ProductEntity> query)
        {
            var culture = Thread.CurrentThread?.CurrentCulture?.Name;
            return query.Select(x => new TranslatedProduct
            {
                Product = x,
                Locale = x.ProductLocales.Any(l => l.Language == culture)
                    ? x.ProductLocales.Single(l => l.Language == culture)
                    : x.ProductLocales.FirstOrDefault(l => l.Language == DeafultCulture)
            });
        }
    }
}
