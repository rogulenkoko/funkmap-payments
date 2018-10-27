using Funkmap.Payments.Core;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Data.Entities;

namespace Funkmap.Payments.Data.Tools
{
    public static class PaymentsStaticDataProdiver
    {
        public static ProductEntity[] Products => new ProductEntity[]
        {
            new ProductEntity
            {
                Name = FunkmapPaymentConstants.ProAccount,
                PaymentType = PaymentType.Subscription,
                HasTrial = false,
                Period = SubscriptionPeriod.Monthly
            },
            new ProductEntity
            {
                Name = FunkmapPaymentConstants.PriorityProfile,
                PaymentType = PaymentType.Subscription,
                HasTrial = false,
                Period = SubscriptionPeriod.Monthly,
            }, 
        };

        public static ProductLocaleEntity[] ProductLocales => new ProductLocaleEntity[]
        {
            #region pro_account

            new ProductLocaleEntity
            {
                Id = 1,
                ProductName = FunkmapPaymentConstants.ProAccount,
                Currency = "RUB",
                Name = "Pro-аккаунт",
                Language = "ru-RU",
                Price = 300,
                Description =
                    "Предоставляет возможность создания более 1 профиля. Применятеся к пользователю ресурса (не к его профилям)."
            },
            new ProductLocaleEntity
            {
                Id = 2,
                ProductName = FunkmapPaymentConstants.ProAccount,
                Currency = "USD",
                Name = "Pro-account",
                Language = "en-US",
                Price = 5,
                Description =
                    "Provides the ability to create more than 1 profile. Apply to the resource user (not to his profiles)."
            },

            #endregion

            #region priority_profile

            new ProductLocaleEntity()
            {
                Id = 3,
                ProductName = FunkmapPaymentConstants.PriorityProfile,
                Name = "Приоритетный профиль",
                Description = "Позволяет выделять один из ваших профилей на карте Bandmap и выдавать одним из первых в поисковых запросах.",
                Currency = "RUB",
                Language = "ru-RU",
                Price = 300
            },

            new ProductLocaleEntity()
            {
                Id = 4,
                ProductName = FunkmapPaymentConstants.PriorityProfile,
                Name = "Priority profile",
                Description = "Allows you to highlight one of your profiles on the Bandmap map and issue one of the first in search queries.",
                Currency = "USD",
                Language = "en-US",
                Price = 5
            }, 

            #endregion
        };
    }
}
