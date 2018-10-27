using System.Threading.Tasks;
using Funkmap.Payments.Core.Parameters;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IPaymentsService
    {
        Task<string> GetOrCreatePayPalPlanIdAsync(CreatePlanParameter productName);
    }
}
