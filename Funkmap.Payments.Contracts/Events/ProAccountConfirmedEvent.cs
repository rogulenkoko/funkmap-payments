namespace Funkmap.Payments.Contracts.Events
{
    public class ProAccountConfirmedEvent
    {
        public ProAccountConfirmedEvent(string login)
        {
            Login = login;
        }

        public string Login { get; set; }
    }
}
