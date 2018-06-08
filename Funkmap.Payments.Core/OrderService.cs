using System;
using System.Threading.Tasks;
using Funkmap.Auth.Client.Abstract;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Core
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentsUnitOfWorkFactory _paymentsUnitOfWorkFactory;
        private readonly IPaymentServiceFacade _paymentService;
        private readonly IUserService _userService;

        public OrderService(IPaymentsUnitOfWorkFactory paymentsUnitOfWorkFactory,
                            IPaymentServiceFacade paymentService,
                            IUserService userService)
        {
            _paymentsUnitOfWorkFactory = paymentsUnitOfWorkFactory;
            _paymentService = paymentService;
            _userService = userService;
        }

        public async Task<CommandResponse> ExecuteOrderAsync(OrderRequest request)
        {

            if (String.IsNullOrEmpty(request?.Login))
            {
                throw new ArgumentException("Invalid user login.");
            }

            if (request.PaymentParameter == null)
            {
                throw new ArgumentException("Invalid payment parameters.");
            }

            using (var unit = _paymentsUnitOfWorkFactory.UnitOfWork())
            using (var transaction = unit.BeginTransaction())
            {
                try
                {
                    var userResponse = await _userService.GetUserAsync(request.Login);

                    if (!userResponse.IsExists)
                    {
                        throw new InvalidOperationException($"User {request.Login} doesn't exists in Bandmap.");
                    }

                    var bandmapUser = userResponse.User;

                    var user = new User
                    {
                        Login = bandmapUser.Login,
                        Email = bandmapUser.Email,
                        Name = bandmapUser.Name
                    };

                    var savedUser = await unit.UserRepository.GetAsync(user.Login);
                    if (savedUser == null)
                    {
                        await unit.UserRepository.CreateAsync(user);
                    }

                    var product = await unit.ProductRepository.GetAsync(request.PaymentRequest.ProductId);
                    if (product == null)
                    {
                        throw new InvalidOperationException($"There is no product with id {request.PaymentRequest.ProductId}.");
                    }

                    var order = new Order
                    {
                        CreatorLogin = user.Login,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        Currency = request.PaymentRequest.Currency,
                        ProductId = product.Id,
                        OrderPrice = product.Price //todo add coupons
                    };
                    
                    await unit.OrderRepository.CreateAsync(order);
                    await unit.ProductRepository.IncrementSellingCountAsync(product.Id);

                    //todo publish to event bus

                    var result = _paymentService.ExecutePayment(order, request.PaymentParameter);
                    if (!result)
                    {
                        throw new InvalidOperationException("Payment execution failed.");
                    }

                    transaction.Commit();

                    return new CommandResponse(true);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new CommandResponse(false) { Error = $"Order processing error. {e.Message}" };
                }
            }
        }
    }

    public abstract class PaymentRequest
    {
        public long ProductId { get; set; }

        public string Currency { get; set; }
    }

    public class PaypalRequest : PaymentRequest
    {

    }
}
