using System;
using System.Data;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Data
{
    internal class UserRepository : IUserRepository
    {
        public UserRepository()
        {
        }

        public async Task CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetAsync(string login)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePaymentInfoAsync(string login, string paymentInfoJson)
        {
            throw new NotImplementedException();
        }
    }
}