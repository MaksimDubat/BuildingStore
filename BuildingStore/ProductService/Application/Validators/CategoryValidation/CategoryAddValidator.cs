using FluentValidation;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;
using ProductService.Application.Validators.CustomValidatorRules;

namespace ProductService.Application.Validators.CategoryValidation
{
    /// <summary>
    /// Валидация добавления категории.
    /// </summary>
    public class CategoryAddValidator : AbstractValidator<AddCategoryCommand>
    {
        public CategoryAddValidator()
        {
            RuleFor(x => x.CategoryName).ValidString();
        }
    }
}
