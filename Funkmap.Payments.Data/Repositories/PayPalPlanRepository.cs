using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Data.Entities;
using Funkmap.Payments.Data.Tools;
using Microsoft.EntityFrameworkCore;

namespace Funkmap.Payments.Data.Repositories
{
    public class PayPalPlanRepository : PaymentsRepositoryBase, IPayPalPlanRepository
    {
        public PayPalPlanRepository(PaymentsContext context) : base(context)
        {
        }

        public async Task CreateAsync(PayPalPlan plan)
        {
            var localeId = await Context.Products.Include(x => x.ProductLocales)
                .Where(x => x.Name == plan.ProductName)
                .GetTranslatedProducts()
                .Select(x=>x.Locale.Id)
                .SingleOrDefaultAsync();

            var entity = new PayPalPlanEntity()
            {
                Id = plan.Id,
                ProductLocaleId = localeId
            };
            await Context.PayPalPlans.AddAsync(entity);
        }

    }
}
