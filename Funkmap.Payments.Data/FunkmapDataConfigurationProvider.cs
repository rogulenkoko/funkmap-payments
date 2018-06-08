using Funkmap.Payments.Core;

namespace Funkmap.Payments.Data
{
    public static class FunkmapDataConfigurationProvider
    {
        public static string ProductsTableName => FunkmapConfigurationProvider.Configuration["Database:ProductTable"];
        public static string UserTableName => FunkmapConfigurationProvider.Configuration["Database:UserTable"];
        public static string OrderTableName => FunkmapConfigurationProvider.Configuration["Database:OrderTable"];
    }
}
