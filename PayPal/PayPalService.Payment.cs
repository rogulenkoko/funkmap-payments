using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PayPal.Abstract;
using PayPal.Contracts;
using PayPal.Exceptions;
using PayPal.Models;
using PayPal.Models.Payment;

namespace PayPal
{
    public partial class PayPalService : IPayPalService, IDisposable
    {
        private readonly PayPalHttpClient _http;
        private string CreatePaymentUrl => "/v1/payments/payment";
        private string ExecutePaymentUrl(string paymentId) => $"/v1/payments/payment/{paymentId}/execute";

        public PayPalService(PayPalConfigurationProvider configurationProvider)
        {
            _http = new PayPalHttpClient(configurationProvider);
        }

        public async Task<PayPalPaymentResult> CreatePaymentAsync(PayPalPayment payment)
        {
            var payPalPaymentCreateRequest = new PayPalPaymentCreateRequest
            {
                Intent = "sale",
                Payer = new PayPalPayer() {PaymentMethod = "paypal"},
                Transactions = new[]
                {
                    new PayPalTransaction
                    {
                        Amount = new PayPalAmount
                        {
                            Total = payment.Total,
                            Currency = payment.Currency
                        }
                    },
                },
                RedirectUrls = new PayPalRedirectUrls() {CancelUrl = payment.CancelUrl, ReturnUrl = payment.ReturnUrl}
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(payPalPaymentCreateRequest),
                Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, CreatePaymentUrl);
            request.Content = requestContent;

            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var payPalResponse = JsonConvert.DeserializeObject<PayPalPaymentCreatedResponse>(content);

            if (!String.IsNullOrWhiteSpace(payPalResponse.ErrorMessage))
            {
                var errorDetails = payPalResponse.ErrorDetailes?.Select(x => new PaypalExceptionErrorDetails
                {
                    Field = x.Field,
                    Issue = x.Issue
                }).ToArray();

                throw new PaypalException(payPalResponse.ErrorMessage, errorDetails, payPalResponse.InformationLink);
            }

            var result = new PayPalPaymentResult
            {
                RedirectUrl = payPalResponse.Links.SingleOrDefault(x => x.Rel == "approval_url")?.Href,
                Id = payPalResponse.Id,
            };

            return result;
        }

        public async Task ExecutePaymentAsync(PayPalExecutePayment payment)
        {
            var url = ExecutePaymentUrl(payment.PaymentId);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var executeRequest = new PayPalPaymentExecuteRequest
            {
                PayerId = payment.PayerId
            };
            request.Content = new StringContent(JsonConvert.SerializeObject(executeRequest), Encoding.UTF8,
                "application/json");

            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            PayPalPaymentExecutedResponse executedPayment =
                JsonConvert.DeserializeObject<PayPalPaymentExecutedResponse>(content);

            if (!String.IsNullOrWhiteSpace(executedPayment.ErrorName))
            {
                throw new PaypalException(executedPayment.ErrorMessage, null, executedPayment.InformationLink);
            }
        }

        public void Dispose()
        {
            _http?.Dispose();
        }
    }
}