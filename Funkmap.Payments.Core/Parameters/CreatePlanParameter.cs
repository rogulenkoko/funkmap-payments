namespace Funkmap.Payments.Core.Parameters
{
    public class CreatePlanParameter
    {
        public string ProductName { get; set; }

        public string ReturnUrl { get; set; }

        public string CancelUrl { get; set; }
    }
}
