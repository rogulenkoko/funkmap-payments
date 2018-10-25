using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Data.Entities;
using Funkmap.Payments.Data.Tools;
using Microsoft.EntityFrameworkCore;

namespace Funkmap.Payments.Data.Repositories
{
    public class ProductRepository : PaymentsRepositoryBase, IProductRepository
    {
        public ProductRepository(PaymentsContext context) : base (context)
        {
        }

        public async Task<Product> GetAsync(string productName)
        {
            var query = Context.Products.Include(x => x.ProductLocales).Where(x=>x.Name == productName);
            var products = GetTranslatedProducts(query);
            return await products.SingleOrDefaultAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var query = Context.Products.Include(x => x.ProductLocales);
            var products = GetTranslatedProducts(query);
            return await products.ToListAsync();
        }

        public async Task<string> GetPlanIdAsync(string productName)
        {
            var query = Context.Products.Include(x => x.ProductLocales)
                .Where(x => x.Name == productName);
            var payPalPlanId = await query.GetTranslatedProducts()
                .Join(Context.PayPalPlans,
                    product => product.Locale.Id,
                    plan => plan.ProductLocaleId,
                    (product, plan) => plan.Id)
                .SingleOrDefaultAsync();
            return payPalPlanId;
        }

        private IQueryable<Product> GetTranslatedProducts(IQueryable<ProductEntity> query)
        {
            return query.GetTranslatedProducts()
                .Select(x => new Product
                {
                    Id = x.Product.Name,
                    Name = x.Locale.Name,
                    Description = x.Locale.Description,
                    Currency = x.Locale.Currency,
                    PaymentType = x.Product.PaymentType,
                    Price = x.Locale.Total,
                    Period = x.Product.Period,
                    HasTrial = x.Product.HasTrial
                });
        }
    }
}