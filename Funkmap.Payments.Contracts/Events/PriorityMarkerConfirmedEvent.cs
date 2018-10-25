using System;

namespace Funkmap.Payments.Contracts.Events
{
    public class PriorityMarkerConfirmedEvent
    {
        public string ProfileLogin { get; set; }

        public DateTime ExpireAtUtc { get; set; }
    }
}
