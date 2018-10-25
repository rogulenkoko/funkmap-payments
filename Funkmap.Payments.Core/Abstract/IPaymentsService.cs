﻿using System.Threading.Tasks;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IPaymentsService
    {
        Task<string> GetOrCreatePayPalPlanIdAsync(string productName);
    }
}