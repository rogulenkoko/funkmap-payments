using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Funkmap.Auth.Client.Abstract;
using Funkmap.Payments.Core.Abstract;

namespace Funkmap.Payments.Core.Services
{
    public class ProductValidationService : IProductValidationService
    {
        private readonly IUserService _userService;

        public ProductValidationService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ValidationResult> ValidateProductAsync(string productName, string influencedLogin)
        {
            switch (productName)
            {
                case FunkmapPaymentConstants.ProAccount: return await ValidateUserAsync(influencedLogin);
                case FunkmapPaymentConstants.PriorityProfile: return await ValidateProfileAsync(influencedLogin);
                default: return new ValidationResult("Invalid product name.");
            }
        }

        private async Task<ValidationResult> ValidateUserAsync(string userLogin)
        {
            return ValidationResult.Success;
            var user = await _userService.GetUserAsync(userLogin);
            if (!user.IsExists)
            {
                return new ValidationResult($"There is no user with login {userLogin}");
            }
            return ValidationResult.Success;
        }

        private async Task<ValidationResult> ValidateProfileAsync(string profileLogin)
        {
            //todo
            return ValidationResult.Success;
        }
    }
}
