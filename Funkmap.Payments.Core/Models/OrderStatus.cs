using System;

namespace Funkmap.Payments.Core.Models
{
    [Flags]
    public enum OrderStatus
    {
        New = 1,

        PendingPayment = 2,

        PaymentReceived = 4,

        PaymentFailed = 8,

        Complete = 16,

        Canceled = 32
    }
}