using System;
using System.Collections.Generic;
using System.Globalization;
using Funkmap.Payments.Core.Abstract;
using PayPal.Api;
using Order = Funkmap.Payments.Core.Order;

namespace Funkmap.Payments.PayPal
{
    public class PayPalPaymentService : IPaymentService
    {
        private readonly PayPalConfigurationProvider _configurationProvider;

        private APIContext Context
        {
            get
            {
                var config = _configurationProvider.PayPalConfiguration;
                var accessToken = new OAuthTokenCredential(config).GetAccessToken();
                return new APIContext(accessToken);
            }
        }

        public PayPalPaymentService(PayPalConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public bool ExecutePayment(Order order, string token)
        {   
            var payment = new Payment()
            {
                intent = "sale",
                payer = new Payer()
                {
                    payment_method = "paypal",
                    //payer_info = new PayerInfo()
                    //{
                    //    email = order.CreatedBy?.Email,
                    //    first_name = order.CreatedBy?.Name
                    //}
                },
                //experience_profile_id = ;
                transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        description = order.Product.Description,
                        amount = new Amount()
                        {
                            currency = order.Currency,
                            total = order.OrderPrice.ToString(CultureInfo.InvariantCulture)
                        },
                        item_list = new ItemList()
                        {
                            items = new List<Item>()
                            {
                                new Item()
                                {
                                    name = order.Product.Name,
                                    description = order.Product.Description,
                                    currency = order.Currency,
                                    price = order.Product.Price.ToString(CultureInfo.InvariantCulture),
                                    quantity = "5",
                                    sku = "sku"
                                }
                            }
                        }
                    }
                },
                redirect_urls = new RedirectUrls
                {
                    return_url = "http://mysite.com/return",
                    cancel_url = "http://mysite.com/cancel"
                }
            };

            var resultPayment = Payment.Create(Context, payment);

            switch (resultPayment.state)
            {
                case "approved": case "created":
                    return true;
                case "failed":
                    return false;
                default:
                    return false;
            }
        }

        public string GetExpiriencedProfile()
        {
            var profile = new WebProfile()
            {
                name = $"bandmap_{Guid.NewGuid()}",
                input_fields = new InputFields
                {
                    no_shipping = 1
                },
                temporary = true
            };

            var response = profile.Create(Context);
            return response.id;
        }
    }
}
