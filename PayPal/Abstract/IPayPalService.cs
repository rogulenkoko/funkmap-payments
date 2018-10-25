using System.Threading.Tasks;
using PayPal.Contracts;

namespace PayPal.Abstract
{
    public interface IPayPalService
    {
        Task<PayPalPaymentResult> CreatePaymentAsync(PayPalPayment payment);

        Task ExecutePaymentAsync(PayPalExecutePayment payment);

        Task CreatePlanAsync(PayPalPlan plan);

        Task ActivatePlanAsync(PayPalPlan plan);

        Task<PayPalAgreementResult> CreateAgreementAsync(PayPalAgreement agreement);

        Task ExecuteAgreementAsync(string token);
    }
}