using System.Threading.Tasks;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IPaymentService
    {
        bool ExecutePayment(Order order, string token);
    }
}
