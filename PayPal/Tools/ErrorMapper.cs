using System.Linq;
using PayPal.Exceptions;
using PayPal.Models;

namespace PayPal.Tools
{
    internal static class ErrorMapper
    {
        internal static PaypalExceptionErrorDetails[] ToModels(this PayPalErrorDetails[] details)
        {
            return details?.Select(x => x.ToModel()).ToArray();
        }

        internal static PaypalExceptionErrorDetails ToModel(this PayPalErrorDetails details)
        {
            if (details == null) return null;
            return new PaypalExceptionErrorDetails
            {
                Issue = details.Issue,
                Field = details.Field
            };
        }
    }
}
