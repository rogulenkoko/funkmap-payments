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
            }
        };

        public static ProductLocaleEntity[] ProductLocales => new ProductLocaleEntity[]
        {
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
            }
        };
    }
}
