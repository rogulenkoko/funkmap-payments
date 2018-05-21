using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Payments.Core;
using Funkmap.Payments.Core.Abstract;
using Newtonsoft.Json.Linq;

namespace Funkmap.Payments.Tests.PaymentService
{
    public class PayPalPaymentServiceTest
    {
        private IPaymentService _paymentService;

        public PayPalPaymentServiceTest()
        {
            
        }

        public void ExecutePaymentTest()
        {
            var order = new Order()
            {
                Product = new Core.Product()
                {
                    Name = "donation",
                    Description = "donation",
                    Price = 0
                },
                CreatedBy = new User()
                {
                    Name = "rogulenkoko",
                    Email = "rogulenkoko@gmail.com",
                    Id = 1
                },
                OrderPrice = 0.5m
            };
            _paymentService.ExecutePayment(order, );
        }

        private async Task<string> GetAccessToken()
        {

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_setting.Value.ClientId}:{_setting.Value.ClientSecret}")));
                var requestBody = new StringContent("grant_type=client_credentials");
                requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var response = await httpClient.PostAsync($"https://api{_setting.Value.EnvironmentUrlPart}.paypal.com/v1/oauth2/token", requestBody);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                dynamic token = JObject.Parse(responseBody);
                string accessToken = token.access_token;
                return accessToken;
            }

            
        }
    }
}
