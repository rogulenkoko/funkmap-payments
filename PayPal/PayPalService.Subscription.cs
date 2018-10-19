using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PayPal.Abstract;
using PayPal.Contracts;
using PayPal.Exceptions;
using PayPal.Models;
using PayPal.Models.Subscription;
using PayPal.Tools;

namespace PayPal
{
    public partial class PayPalService : IPayPalService, IDisposable
    {
        private string CreatePlanUrl => "/v1/payments/billing-plans/";

        public async Task<PayPalPaymentResult> CreateSubscription(PayPalPayment payment)
        {
            throw new NotImplementedException();
        }

        public async Task CreatePlanAsync(PayPalPlan plan)
        {
            var createPlanRequest = new PayPalPlanCreateRequest()
            {
                Name = plan.Name,
                Description = plan.Description,
                Type = "fixed",
                PaymentDefinitions = new []
                {
                    new PayPalPaymentDefinition
                    {
                        Frequency = GetFrequency(plan.PeriodType),
                        FrequencyInterval = plan.Frequency.ToString(),
                        Amount = new PayPalAmount
                        {
                            Currency = plan.Currency,
                            Total = plan.Total.ToString(CultureInfo.InvariantCulture),
                        },
                        Name = "Regular payment definition",
                        Type = "REGULAR",
                        Cycles = "12",
                        ChargeModels = new []
                        {
                            new PayPalChargeModel()
                            {
                                Amount = new PayPalAmount
                                {
                                    Total = Decimal.Multiply(plan.Total, 0.01m).ToString(CultureInfo.InvariantCulture),
                                    Currency = plan.Currency
                                },
                                Type = "SHIPPING"
                            }
                        }
                    }
                },
                MerchantPreferences = new PayPalMerchantPreferences()
                {
                    ReturnUrl = "http://localhost:6000/return",
                    CancelUrl = "http://localhost:6000/cancel",
                    InitialFailAmountAction = "CONTINUE",
                    MaxFailAttempts = "3",
                    AutoBillAmount = "YES"
                }
            };

            var jsonContent = JsonConvert.SerializeObject(createPlanRequest);
            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, CreatePlanUrl);
            request.Content = requestContent;

            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var createdPlan = JsonConvert.DeserializeObject<PayPalPlanCreateResponse>(content);

            if (!String.IsNullOrWhiteSpace(createdPlan.ErrorMessage))
            {
                throw new PaypalException(createdPlan.ErrorMessage, createdPlan.ErrorDetailes.ToModels(), createdPlan.InformationLink);
            }
        }

        private string GetFrequency(PeriodType periodType)
        {
            switch (periodType)
            {
                case PeriodType.Day: return "DAY";
                case PeriodType.Month: return "MONTH";
                case PeriodType.Year: return "YEAR";

                default: throw new InvalidOperationException("Invalid period type");
            }
        }
    }
}