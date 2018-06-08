using System.Threading.Tasks;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);

        Task<User> GetAsync(string login);

        Task<bool> UpdatePaymentInfoAsync(string login, string paymentInfoJson);
    }
}
