using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Data.Entities;

namespace Funkmap.Payments.Data.Tools
{
    public static class PaymentsStaticDatProdiver
    {
        public static ProductEntity[] Products => new ProductEntity[]
        {
            new ProductEntity
            {
                Name = "pro_account",
                PaymentType = PaymentType.Subscribtion,
                HasTrial = false,
                Period = SubscribtionPeriod.Monthly
            },
            new ProductEntity
            {
                Name = "priority_profile",
                PaymentType = PaymentType.Subscribtion,
                HasTrial = false,
                Period = SubscribtionPeriod.Monthly,
            }, 
        };

        public static ProductLocaleEntity[] ProductLocales => new ProductLocaleEntity[]
        {
            #region pro_account

            new ProductLocaleEntity
            {
                Id = 1,
                ProductName = "pro_account",
                Currency = "RUB",
                Name = "Pro-аккаунт",
                Language = "ru-RU",
                Total = 300,
                Description =
                    "Предоставляет возможность создания более 1 профиля. Применятеся к пользователю ресурса (не к его профилям)."
            },
            new ProductLocaleEntity
            {
                Id = 2,
                ProductName = "pro_account",
                Currency = "USD",
                Name = "Pro-account",
                Language = "en-US",
                Total = 5,
                Description =
                    "Provides the ability to create more than 1 profile. Apply to the resource user (not to his profiles)."
            },

            #endregion

            #region priority_profile

            new ProductLocaleEntity()
            {
                Id = 3,
                ProductName = "priority_profile",
                Name = "Приоритетный профиль",
                Description = "Позволяет выделять один из ваших профилей на карте Bandmap и выдавать одним из первых в поисковых запросах.",
                Currency = "RUB",
                Language = "ru-RU",
                Total = 300
            },

            new ProductLocaleEntity()
            {
                Id = 4,
                ProductName = "priority_profile",
                Name = "Priority profile",
                Description = "Allows you to highlight one of your profiles on the Bandmap map and issue one of the first in search queries.",
                Currency = "USD",
                Language = "en-US",
                Total = 5
            }, 

            #endregion
        };
    }
}
