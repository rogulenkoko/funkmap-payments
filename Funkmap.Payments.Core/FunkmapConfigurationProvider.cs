using Microsoft.Extensions.Configuration;

namespace Funkmap.Payments.Core
{
    public static class FunkmapConfigurationProvider
    {
        private static IConfiguration _configuration;

        public static IConfiguration Configuration
        {
            get => _configuration;
            set
            {
                if (_configuration == null) _configuration = value;
            }
        }
    }
}
