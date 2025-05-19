using FluentValidation;
using NotificationService.Application.MediatConfiguration.Commands;
using NotificationService.Application.Validators.CustomValidatorRules;

namespace NotificationService.Application.Validators.MessageValidator
{
    /// <summary>
    /// Валидация добавления сообщения.
    /// </summary>
    public class AddMessageValidator : AbstractValidator<AddMessageCommand>
    {
        public AddMessageValidator()
        {
            RuleFor(x => x.Model.Subject).ValidString();

            RuleFor(x => x.Model.Body).ValidString();
        }
    }
}
