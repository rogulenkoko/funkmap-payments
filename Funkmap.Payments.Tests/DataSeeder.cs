using Funkmap.Payments.Data;
using Funkmap.Payments.Data.Tools;
using Microsoft.EntityFrameworkCore;

namespace Funkmap.Payments.Tests
{
    public static class DataSeeder
    {
        public static void Seed(DbContextOptions<PaymentsContext> options)
        {
            using (var context = new PaymentsContext(options))
            {
                context.ProductLocales.AddRange(PaymentsStaticDataProdiver.ProductLocales);
                context.Products.AddRange(PaymentsStaticDataProdiver.Products);
                context.SaveChanges();
            }
        }
    }
}
