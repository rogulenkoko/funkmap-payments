using System.Threading.Tasks;
using PayPal.Contracts;

namespace PayPal.Abstract
{
    public interface IPayPalService
    {
        Task<PayPalPaymentResult> CreatePaymentAsync(PayPalPayment payment);

        Task ExecutePaymentAsync(PayPalExecutePayment payment);
    }
}