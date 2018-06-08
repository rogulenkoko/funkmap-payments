using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IPaymentService<T> where T : IPaymentParameter
    {
        bool ExecutePayment(Order order, T parameter);
    }
}
