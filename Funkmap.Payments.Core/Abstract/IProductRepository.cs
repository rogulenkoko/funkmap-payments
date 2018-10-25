using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();

        Task<Product> GetAsync(string productName);

        Task<string> GetPlanIdAsync(string productName);
    }
}
