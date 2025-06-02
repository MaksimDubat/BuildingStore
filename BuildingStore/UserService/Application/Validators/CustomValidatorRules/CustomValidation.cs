using FluentValidation;

namespace UserService.Application.Validators.CustomValidatorRules
{
    /// <summary>
    /// Валиация строки и email для переиспользования.
    /// </summary>
    public static class CustomValidation
    {
        public static IRuleBuilderOptions<T, string> ValidString<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .Matches(@"^[a-zA-Zа-яА-Я\s]+$").WithMessage("Only Latin or Cyrillic")
                .Matches(@"^[^<>]+$").WithMessage("not alloed < or >")
                .MinimumLength(3).WithMessage("no smaller than 3")
                .MaximumLength(500).WithMessage("no longer than 100");
        }

        public static IRuleBuilderOptions<T, string> ValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .EmailAddress().WithMessage("Wrong format")
                .Matches(@"^[a-zA-Z0-9@.]+$").WithMessage("Only english letters and @")
                .Matches(@"^[^<>]+$").WithMessage("not alloed < or >")
                .MinimumLength(3).WithMessage("no smaller than 3")
                .MaximumLength(500).WithMessage("no longer than 100");
        }
    }
}
