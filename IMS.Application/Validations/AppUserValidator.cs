using FluentValidation;
using IMS.Application.DTOs.Users;
using IMS.Core.Constants;

namespace IMS.Application.Validations;
public class AppUserValidator
    : AbstractValidator<RegisterUserDto>
{
    private const int MinPasswordLength = 6;
    private const int MaxPasswordLength = 10;
    public AppUserValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Username is required");

        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("Email is required")
            .EmailAddress()
                .WithMessage("Email format is not valid")
            .Must(email => email.EndsWith("@quizplus.com"))
                .WithMessage("Email must be a company email (@quizplus.com)");

        RuleFor(x => x.Password).NotEmpty()
         .WithMessage("Password is required")
     .MinimumLength(MinPasswordLength)
         .WithMessage($"Password must be at least {MinPasswordLength} characters")
     .MaximumLength(MaxPasswordLength)
         .WithMessage($"Password must not exceed {MaxPasswordLength} characters");


        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role is required");

        When(x =>
            string.Equals(x.Role, StaticRole.Trainee, StringComparison.OrdinalIgnoreCase),
            () =>
            {
                RuleFor(x => x.GraduationYear)
                    .NotNull()
                        .WithMessage("Graduation year is required for trainees")
                    .InclusiveBetween(2024, DateTime.Now.Year)
                        .WithMessage($"Graduation year must be between 2024 and {DateTime.Now.Year}");
            });
    }
}
