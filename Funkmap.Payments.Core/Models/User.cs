namespace Funkmap.Payments.Core.Models
{
    public class User
    {
        public string Login { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string PaymentInfoJson { get; set; }
    }
}
