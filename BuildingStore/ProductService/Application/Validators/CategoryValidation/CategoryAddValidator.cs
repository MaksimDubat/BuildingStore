using FluentValidation;
using ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Commands;

namespace ProductService.Application.Validators.CategoryValidation
{
    /// <summary>
    /// Валидация добавления категории.
    /// </summary>
    public class CategoryAddValidator : AbstractValidator<AddCategoryCommand>
    {
        public CategoryAddValidator()
        { 
            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .Matches(@"^[a-zA-Zа-яА-Я\s]+$").WithMessage("Only Latin or Cyrillic")
                .Matches(@"^[^<>]+$").WithMessage("not alloed < or >") 
                .MinimumLength(3).WithMessage("no smaller than 3")
                .MaximumLength(20).WithMessage("no longer than 20");
        }
    }
}
