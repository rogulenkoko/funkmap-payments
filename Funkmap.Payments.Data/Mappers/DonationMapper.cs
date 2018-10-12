using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Data.Entities;

namespace Funkmap.Payments.Data.Mappers
{
    internal static class DonationMapper 
    {
        public static DonationEntity ToEntity(this Donation source)
        {
            if (source == null) return null;
            return new DonationEntity()
            {
                Currency = source.Currency,
                Total = source.Total,
                DateUtc = source.DateUtc,
                Id = source.Id
            };
        }

        public static Donation ToModel(this DonationEntity source)
        {
            if (source == null) return null;
            return new Donation()
            {
                Currency = source.Currency,
                Total = source.Total,
                DateUtc = source.DateUtc,
                Id = source.Id
            };
        }
    }
}
