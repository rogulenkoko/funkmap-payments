using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace Funkmap.Payments.Tools
{
    public static class LocaleMiddleware
    {
        private static readonly HashSet<string> Locales;
        private static readonly string DefaultLanguage;

        static LocaleMiddleware()
        {
            DefaultLanguage = "en-US";

            Locales = new HashSet<string>
            {
                { "ru-RU"},
                { "en-US"}
            };
        }

        public static void Invoke(HttpContext context)
        {
            var languageHeader = context.Request.Headers["Accept-Language"].FirstOrDefault();
            var lang = languageHeader == null || !Locales.Contains(languageHeader) ? DefaultLanguage : languageHeader;

            var culture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
