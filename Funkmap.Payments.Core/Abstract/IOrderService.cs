using System.Threading.Tasks;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Core
{
    public interface IOrderService
    {
        Task<CommandResponse> ExecuteOrderAsync(OrderRequest request);
    }
}