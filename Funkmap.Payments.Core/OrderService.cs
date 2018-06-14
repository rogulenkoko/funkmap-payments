using System;
using System.Threading.Tasks;
using Funkmap.Auth.Client.Abstract;
using Funkmap.Cqrs.Abstract;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Core.Events;
using Funkmap.Payments.Core.Models;

namespace Funkmap.Payments.Core
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentsUnitOfWorkFactory _paymentsUnitOfWorkFactory;
        private readonly IPaymentServiceFacade _paymentService;
        private readonly IEventsFactory _eventsFactory;
        private readonly IUserService _userService;
        private readonly IEventBus _eventBus;


        public OrderService(IPaymentsUnitOfWorkFactory paymentsUnitOfWorkFactory,
                            IPaymentServiceFacade paymentService,
                            IEventsFactory eventsFactory,
                            IUserService userService,
                            IEventBus eventBus)
        {
            _paymentsUnitOfWorkFactory = paymentsUnitOfWorkFactory;
            _paymentService = paymentService;
            _eventsFactory = eventsFactory;
            _userService = userService;
            _eventBus = eventBus;
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
                    //Get user full information
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

                    //Save user if not exists
                    var savedUser = await unit.UserRepository.GetAsync(user.Login);
                    if (savedUser == null)
                    {
                        await unit.UserRepository.CreateAsync(user);
                    }

                    //Get requested product
                    var product = await unit.ProductRepository.GetAsync(request.PaymentRequest.ProductId);
                    if (product == null)
                    {
                        throw new InvalidOperationException($"There is no product with id {request.PaymentRequest.ProductId}.");
                    }

                    if (request.PaymentRequest.ProductName != product.Name)
                    {
                        request.PaymentRequest.ProductName = product.Name;
                    }

                    //Create and save order
                    var order = new Order
                    {
                        CreatorLogin = user.Login,
                        Creator = user,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        Currency = request.PaymentRequest.Currency,
                        ProductId = product.Id,
                        Product = product,
                        OrderPrice = product.Price //todo add coupons
                    };

                    await unit.OrderRepository.CreateAsync(order);
                    await unit.ProductRepository.IncrementSellingCountAsync(product.Id);

                    //Notify Bandmap about the deal
                    var @event = _eventsFactory.CreatEvent(product.Name, request.PaymentRequest.ProductParameter);

                    await _eventBus.PublishAsync(@event);

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


}
