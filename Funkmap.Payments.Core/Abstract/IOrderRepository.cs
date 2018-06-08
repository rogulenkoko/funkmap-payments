
using System.Threading.Tasks;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IOrderRepository
    {
        Task<bool> CreateAsync(Order order);

        Task<Order> GetAsync(long id);
    }
}
