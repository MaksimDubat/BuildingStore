using FluentValidation;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Application.Validators.CustomValidatorRules;

namespace UserService.Application.Validators.UserValidation
{
    /// <summary>
    /// Валидация входа пользователей.
    /// </summary>
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Model.Email).ValidEmail();

            RuleFor(x => x.Model.Password)
                .NotEmpty()
                .MinimumLength(4).WithMessage("no smaller than 4");
        }
    }
}
