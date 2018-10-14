using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Data.Entities;

namespace Funkmap.Payments.Data.Mappers
{
    internal static class PaymentMapper
    {
        public static PaymentEntity ToEntity(this Payment source)
        {
            if (source == null) return null;
            return new PaymentEntity()
            {
                Currency = source.Currency,
                Total = source.Total,
                DateUtc = source.DateUtc,
                Id = source.Id,
                PaymentStatus = source.PaymentStatus,
                ExternalId = source.ExternalId
            };
        }

        public static Payment ToModel(this PaymentEntity source)
        {
            if (source == null) return null;
            return new Payment()
            {
                Currency = source.Currency,
                Total = source.Total,
                DateUtc = source.DateUtc,
                Id = source.Id,
                PaymentStatus = source.PaymentStatus,
                ExternalId = source.ExternalId
            };
        }
    }
}