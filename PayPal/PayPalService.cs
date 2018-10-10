using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PayPal.Abstract;
using PayPal.Contracts;
using PayPal.Models;

namespace PayPal
{
    public class PayPalService : IPayPalService, IDisposable
    {
        private readonly PayPalHttpClient _http;
        private readonly string CreatePaymentUrl = "/v1/payments/payment";

        public PayPalService(PayPalConfigurationProvider configurationProvider)
        {
            _http = new PayPalHttpClient(configurationProvider);
        }

        public async Task<PayPalPaymentResult> CreatePaymentAsync(PayPalPayment payment)
        {
            var payPalPaymentCreateRequest = new PayPalPaymentCreateRequest
            {
                Intent = "sale",
                Payer = new PayPalPayer() { PaymentMethod = "paypal" },
                Transactions = new PayPalTransaction[]
                {
                    new PayPalTransaction()
                    {
                        Amount = new PayPalAmount()
                        {
                            Total = payment.Total,
                            Currency = payment.Currency
                        }
                    },
                },
                RedirectUrls = new PayPalRedirectUrls() { CancelUrl = payment.CancelUrl, ReturnUrl = payment.ReturnUrl }
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(payPalPaymentCreateRequest), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, CreatePaymentUrl);
            request.Content = requestContent;

            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var payPalResponse = JsonConvert.DeserializeObject<PayPalPaymentCreatedResponse>(content);

            var result = new PayPalPaymentResult
            {
                PayPalRedirectUrl = payPalResponse.Links.SingleOrDefault(x=>x.Rel == "approval_url")?.Href
            };

            return result;
        }

        public async Task ExecutePaymentAsync()
        {

        }

        public void Dispose()
        {
            _http?.Dispose();
        }
    }
}
