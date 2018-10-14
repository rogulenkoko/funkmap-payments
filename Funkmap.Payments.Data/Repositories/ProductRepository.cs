using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Funkmap.Payments.Data.Repositories
{
    public class ProductRepository : PaymentsRepositoryBase, IProductRepository
    {
        public ProductRepository(PaymentsContext context) : base (context)
        {
        }
        
        public async Task<List<Product>> GetAllAsync()
        {
            var culture = Thread.CurrentThread?.CurrentCulture?.Name;

            if (String.IsNullOrEmpty(culture))
            {
                culture = "en_US";
            }

            var products = await Context.Products.Include(x => x.ProductLocales)
                .Select(x=> new
                {
                    Product = x,
                    Locale = x.ProductLocales.SingleOrDefault(l=> l.Language == culture)
                })
                .Select(x=> new Product
                {
                    Id = x.Product.Name,
                    Name = x.Locale.Name,
                    Description = x.Locale.Description,
                    Currency = x.Locale.Currency,
                    PaymentType = x.Product.PaymentType,
                    Price = x.Locale.Total,
                    Period = x.Product.Period,
                    HasTrial = x.Product.HasTrial
                })
                .ToListAsync();

            return products;
        }
    }
}