using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IProductValidationService
    {
        Task<ValidationResult> ValidateProductAsync(string productName, string influencedLogin);
    }
}
