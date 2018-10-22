using System;

namespace Funkmap.Payments.Contracts.Events
{
    public class ProAccountConfirmedEvent
    {
        public string Login { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}
