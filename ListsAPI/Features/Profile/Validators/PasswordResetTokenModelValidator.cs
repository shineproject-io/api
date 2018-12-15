using FluentValidation;
using ListsAPI.Features.Profile.RequestModels;

namespace ListsAPI.Features.Profile.Validators
{
    public class PasswordResetTokenRequestModelValidator : AbstractValidator<PasswordResetTokenRequest>
    {
        public PasswordResetTokenRequestModelValidator()
        {
            RuleFor(reg => reg.EmailAddress).NotEmpty();
        }
    }
}