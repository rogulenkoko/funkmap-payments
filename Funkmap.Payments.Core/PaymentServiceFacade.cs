using System;
using Autofac;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Core
{
    public interface IPaymentServiceFacade
    {
        bool ExecutePayment<T>(Order order, T parameter) where T : IPaymentParameter;
    }

    public class PaymentServiceFacade : IPaymentServiceFacade
    {
        private readonly IComponentContext _componentContext;

        public PaymentServiceFacade(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public bool ExecutePayment<T>(Order order, T parameter) where T : IPaymentParameter
        {
            if (!_componentContext.IsRegistered<IPaymentService<T>>())
            {
                throw new InvalidOperationException($"Payment service with generic parameter {typeof(T).Name} is not registered.");
            }

            var paymentService = _componentContext.Resolve<IPaymentService<T>>();
            return paymentService.ExecutePayment(order, parameter);
        }
    }
}
