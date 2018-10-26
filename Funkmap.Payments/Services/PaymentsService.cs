﻿using System;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;
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

        public async Task<string> GetOrCreatePayPalPlanIdAsync(string productName)
        {
            var planId = await _paymentsUnitOfWork.PayPalPlanRepository.GetPlanIdAsync(productName);
            if (!string.IsNullOrEmpty(planId)) return planId;

            var product = await _paymentsUnitOfWork.ProductRepository.GetAsync(productName);
            var payPalPlan = new PayPalPlan
            {
                Name = product.Name,
                Description = product.Description,
                Currency = product.Currency,
                Frequency = 1,
                PeriodType = ToPeriodType(product.Period),
                Total = product.Price,
            };
            await _payPalService.CreatePlanAsync(payPalPlan);
            await _payPalService.ActivatePlanAsync(payPalPlan);

            var plan = new Core.Models.PayPalPlan
            {
                Id = payPalPlan.Id,
                ProductName = productName,
            };
            await _paymentsUnitOfWork.PayPalPlanRepository.CreateAsync(plan);
            await _paymentsUnitOfWork.SaveAsync();

            return payPalPlan.Id;
        }

        private PeriodType ToPeriodType(SubscribtionPeriod periodType)
        {
            switch (periodType)
            {
                case SubscribtionPeriod.Monthly:
                    return PeriodType.Month;
                case SubscribtionPeriod.Daily:
                    return PeriodType.Day;
                case SubscribtionPeriod.Yearly:
                    return PeriodType.Year;
                default: throw new InvalidOperationException($"Can't convert {nameof(SubscribtionPeriod)} to {nameof(PeriodType)}");

            }
        }
    }
}
