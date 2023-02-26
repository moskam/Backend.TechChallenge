using Backend.TechChallenge.Api.DAL;
using Backend.TechChallenge.Contracts.Features.Users;
using FluentValidation;

namespace Backend.TechChallenge.Api.Features.Users.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("The name is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("The email is required")
            .DependentRules(() =>
            {
                RuleFor(x => x.Email).EmailAddress()
                    .WithMessage(x => $"Invalid email value {x.Email}");
            });

        RuleFor(x => x.Address).NotEmpty().WithMessage("The address is required");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("The phone is required")
            .DependentRules(() =>
            {
                RuleFor(x => x.Phone).Matches("^[+]?[0-9 ]{5,30}$") // just madeup rule, phone number should be all numbers and it can start with + sign
                    .WithMessage(x => $"Invalid phone number value {x.Phone}");
            });

        When(x => !string.IsNullOrEmpty(x.Money), () =>
        {
            RuleFor(x => x.Money).Must(x => decimal.TryParse(x, out var temp) && temp >= 0)
                .WithMessage(x => $"Invalid money value {x.Money}");
        });
        When(x => !string.IsNullOrEmpty(x.UserType), () =>
        {
            RuleFor(x => x.UserType).IsEnumName(typeof(UserType))
                .WithMessage(x => $"Invalid user type value {x.UserType}");
        });
    }
}
