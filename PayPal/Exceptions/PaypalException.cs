using System;

namespace PayPal.Exceptions
{
    public class PaypalException : Exception
    {
        public PaypalException(string errorMessage, PaypalExceptionErrorDetails[] errorDetails = null, string informationLink = null)
        {
            ErrorMessage = errorMessage;
            InformationLink = informationLink;
            ErrorDetailes = errorDetails;
        }

        public string ErrorMessage { get; set; }
        
        public string InformationLink { get; set; }
        
        public PaypalExceptionErrorDetails[] ErrorDetailes { get; set; }
    }

    public class PaypalExceptionErrorDetails
    {
        public string Field { get; set; }
        
        public string Issue { get; set; }
    }
}
