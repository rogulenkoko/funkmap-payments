using System;

namespace Funkmap.Payments.Core.Events
{
    public interface IEventsFactory
    {
        IPaymentEvent CreatEvent(string productName, ProductParameterBase productParameter = null);
    }

    public interface IPaymentEvent
    {
        
    }

    public class EventsFactory : IEventsFactory
    {
        public IPaymentEvent CreatEvent(string productName, ProductParameterBase productParameter = null)
        {
            switch (productName)
            {
                case "proaccount":
                    var proAccountParameter = productParameter as ProAccountParameter;

                    if (proAccountParameter == null)
                    {
                        throw new InvalidOperationException("Invalid product type and product parameter.");
                    }

                    return new ProAccountConfirmedEvent()
                    {
                        Login = proAccountParameter.Login
                    };

                default: return null;
            }
        }
    }
}
