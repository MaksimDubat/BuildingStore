using FluentValidation;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Application.Validators.CustomValidatorRules;

namespace UserService.Application.Validators.UserValidation
{
    /// <summary>
    /// Валидация регистриации менеджеров.
    /// </summary>
    public class RegistrationManagerValidator : AbstractValidator<RegisterManagersCommand>
    {
        public RegistrationManagerValidator()
        {
            RuleFor(x => x.Model.Name).ValidString();

            RuleFor(x => x.Model.Email).ValidEmail();

            RuleFor(x => x.Model.Password)
                .NotEmpty()
                .MinimumLength(4).WithMessage("no smaller than 4");

            RuleFor(x => x.Model.ConfirmPassword)
                .NotEmpty()
                .MinimumLength(4).WithMessage("no smaller than 4");
        }
    }
}
