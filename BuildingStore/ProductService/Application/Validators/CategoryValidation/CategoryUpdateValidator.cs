using FluentValidation;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;
using ProductService.Application.Validators.CustomValidatorRules;

namespace ProductService.Application.Validators.CategoryValidation
{
    /// <summary>
    /// Валидация обновления товара.
    /// </summary>
    public class CategoryUpdateValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public CategoryUpdateValidator()
        {
            RuleFor(x => x.CategoryName).ValidString();
        }
    }
}
