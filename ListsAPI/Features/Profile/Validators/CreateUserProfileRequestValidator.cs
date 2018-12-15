using FluentValidation;
using ListsAPI.Features.Profile.RequestModels;

namespace ListsAPI.Features.Profile.Validators
{
    public class CreateUserProfileRequestValidator : AbstractValidator<CreateUserProfileRequest>
    {
        public CreateUserProfileRequestValidator()
        {
            RuleFor(req => req.EmailAddress).NotEmpty();
            RuleFor(req => req.GivenName).NotEmpty();
            RuleFor(req => req.FamilyName).NotEmpty();
            RuleFor(req => req.Password).NotEmpty();
            RuleFor(req => req.ProfilePicturePath).NotEmpty();
        }
    }
}