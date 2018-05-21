using Funkmap.Payments.Core;

namespace Funkmap.Payments.Data
{
    public static class FunkmapDataConfigurationProvider
    {
        public static string ProductsTableName => FunkmapConfigurationProvider.Configuration["Database:ProductTable"];
    }
}
