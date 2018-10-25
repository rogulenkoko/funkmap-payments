using System;
using System.Linq;
using System.Net;
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
        private string PlanUrl => "/v1/payments/billing-plans/";
        private string AgreementUrl => "v1/payments/billing-agreements/";

        public async Task<PayPalPaymentResult> CreateSubscription(PayPalPayment payment)
        {
            throw new NotImplementedException();
        }

        public async Task CreatePlanAsync(PayPalPlan plan)
        {
            var createPlanRequest = new PayPalPlanCreateRequest
            {
                Name = plan.Name,
                Description = plan.Description,
                Type = "fixed",
                PaymentDefinitions = new[]
                {
                    new PayPalPaymentDefinition
                    {
                        Frequency = GetFrequency(plan.PeriodType),
                        FrequencyInterval = plan.Frequency.ToString(),
                        Amount = new PayPalPlanAmount
                        {
                            Currency = plan.Currency,
                            Total = plan.Total,
                        },
                        Name = "Regular payment definition",
                        Type = "REGULAR",
                        Cycles = "12",
                    }
                },
                MerchantPreferences = new PayPalMerchantPreferences
                {
                    ReturnUrl = "http://localhost:6000/return",
                    CancelUrl = "http://localhost:6000/cancel",
                    InitialFailAmountAction = "CONTINUE",
                    MaxFailAttempts = "3",
                    AutoBillAmount = "YES",
                    SetupFee = new PayPalPlanAmount
                    {
                        Currency = plan.Currency,
                        Total = Decimal.Multiply(plan.Total, 0.01m),
                    }
                }
            };

            var jsonContent = JsonConvert.SerializeObject(createPlanRequest);
            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, PlanUrl);
            request.Content = requestContent;

            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var createdPlan = JsonConvert.DeserializeObject<PayPalPlanCreateResponse>(content);

            if (!String.IsNullOrWhiteSpace(createdPlan.ErrorMessage))
            {
                throw new PaypalException(createdPlan.ErrorMessage, createdPlan.ErrorDetailes.ToModels(), createdPlan.InformationLink);
            }

            plan.Id = createdPlan.Id;
        }

        public async Task ActivatePlanAsync(PayPalPlan plan)
        {
            var payPalPatchModel = new PayPalPatchModel()
            {
                PayPalPatchValue = new PayPalPatchValue()
                {
                    State = "ACTIVE"
                },
                Operation = "replace",
                Path = "/"
            };

            var jsonContent = JsonConvert.SerializeObject(new[] { payPalPatchModel });
            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{PlanUrl}{plan.Id}");
            request.Content = requestContent;

            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK && String.IsNullOrEmpty(content))
            {
                return;
            }

            var createdPlan = JsonConvert.DeserializeObject<PayPalPlanCreateResponse>(content);
            if (!String.IsNullOrWhiteSpace(createdPlan.ErrorMessage))
            {
                throw new PaypalException(createdPlan.ErrorMessage, createdPlan.ErrorDetailes.ToModels(), createdPlan.InformationLink);
            }
        }

        public async Task<PayPalAgreementResult> CreateAgreementAsync(PayPalAgreement agreement)
        {
            var payPalAgreementRequest = new PayPalAgreementRequest(agreement.PayPalPlanId)
            {
                Name = agreement.Name,
                Description = agreement.Description
            };

            var jsonContent = JsonConvert.SerializeObject(payPalAgreementRequest);
            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, AgreementUrl);
            request.Content = requestContent;

            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var createdAgreement = JsonConvert.DeserializeObject<PayPalAgreementCreateResponse>(content);
            if (!String.IsNullOrWhiteSpace(createdAgreement.ErrorMessage))
            {
                throw new PaypalException(createdAgreement.ErrorMessage, createdAgreement.ErrorDetailes.ToModels(), createdAgreement.InformationLink);
            }

            return new PayPalAgreementResult
            {
                RedirectUrl = createdAgreement.Links.SingleOrDefault(x => x.Rel == "approval_url")?.Href,
            };
        }

        public async Task ExecuteAgreementAsync(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{AgreementUrl}{token}/agreement-execute");
            request.Content = new StringContent(String.Empty, Encoding.UTF8, "application/json");
            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
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