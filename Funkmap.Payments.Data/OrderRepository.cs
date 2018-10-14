using System;
using System.Data;
using System.Threading.Tasks;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Data
{
    internal class OrderRepository : IOrderRepository
    {
        public OrderRepository()
        {
        }

        public async Task<bool> CreateAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}