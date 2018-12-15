using FluentValidation;
using ListsAPI.Features.Authentication.RequestModels;

namespace ListsAPI.Features.Authentication.Validators
{
    public class SignInRequestValidator : AbstractValidator<SignInRequest>
    {
        public SignInRequestValidator()
        {
            RuleFor(req => req.EmailAddress).NotEmpty();
            RuleFor(req => req.Password).NotEmpty();
        }
    }
}