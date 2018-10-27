using System;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Core.Parameters;
using PayPal.Abstract;
using PayPal.Contracts;
using PayPalPlan = PayPal.Contracts.PayPalPlan;

namespace Funkmap.Payments.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IPayPalService _payPalService;
        private readonly IPaymentsUnitOfWork _paymentsUnitOfWork;

        public PaymentsService(IPayPalService payPalService, IPaymentsUnitOfWork paymentsUnitOfWork)
        {
            _payPalService = payPalService;
            _paymentsUnitOfWork = paymentsUnitOfWork;
        }

        public async Task<string> GetOrCreatePayPalPlanIdAsync(CreatePlanParameter parameter)
        {
            var planId = await _paymentsUnitOfWork.PayPalPlanRepository.GetPlanIdAsync(parameter.ProductName);
            if (!string.IsNullOrEmpty(planId)) return planId;

            var product = await _paymentsUnitOfWork.ProductRepository.GetAsync(parameter.ProductName);
            var payPalPlan = new PayPalPlan
            {
                Name = product.Title,
                Description = product.Description,
                Currency = product.Currency,
                Frequency = 1,
                PeriodType = ToPeriodType(product.Period),
                Total = product.Price,
                ReturnUrl = parameter.ReturnUrl,
                CancelUrl = parameter.CancelUrl
            };
            await _payPalService.CreatePlanAsync(payPalPlan);
            await _payPalService.ActivatePlanAsync(payPalPlan);

            var plan = new Core.Models.PayPalPlan
            {
                Id = payPalPlan.Id,
                ProductName = parameter.ProductName,
            };
            await _paymentsUnitOfWork.PayPalPlanRepository.CreateAsync(plan);
            await _paymentsUnitOfWork.SaveAsync();

            return payPalPlan.Id;
        }

        private PeriodType ToPeriodType(SubscriptionPeriod periodType)
        {
            switch (periodType)
            {
                case SubscriptionPeriod.Monthly:
                    return PeriodType.Month;
                case SubscriptionPeriod.Daily:
                    return PeriodType.Day;
                case SubscriptionPeriod.Yearly:
                    return PeriodType.Year;
                default: throw new InvalidOperationException($"Can't convert {nameof(SubscriptionPeriod)} to {nameof(PeriodType)}");

            }
        }
    }
}
