namespace Funkmap.Payments.Contracts.Events
{
    public class PriorityMarkerConfirmedEvent
    {
        public PriorityMarkerConfirmedEvent(string profileLogin)
        {
            ProfileLogin = profileLogin;
        }

        public string ProfileLogin { get; set; }
    }
}
