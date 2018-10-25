using System.Threading.Tasks;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IPayPalPlanRepository
    {
        Task CreateAsync(PayPalPlan plan);
    }
}
