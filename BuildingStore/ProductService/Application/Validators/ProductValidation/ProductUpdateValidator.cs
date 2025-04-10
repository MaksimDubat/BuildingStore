using FluentValidation;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;

namespace ProductService.Application.Validators.ProductValidation
{
    /// <summary>
    /// Валидация обновления продукта.
    /// </summary>
    public class ProductUpdateValidator : AbstractValidator<UpdateProductCommand>
    {
        public ProductUpdateValidator()
        {
            RuleFor(x => x.Product.Name)
                .NotEmpty()
                .Matches(@"^[a-zA-Zа-яА-Я\s]+$").WithMessage("Only Latin or Cyrillic")
                .Matches(@"^[^<>]+$").WithMessage("not alloed < or >")
                .MinimumLength(3).WithMessage("no smaller than 3")
                .MaximumLength(100).WithMessage("no longer than 100");

            RuleFor(x => x.Product.Description)
                .NotEmpty()
                .Matches(@"^[a-zA-Zа-яА-Я\s]+$").WithMessage("Only Latin or Cyrillic")
                .Matches(@"^[^<>]+$").WithMessage("not alloed < or >")
                .MinimumLength(3).WithMessage("no smaller than 3")
                .MaximumLength(500).WithMessage("no longer than 500");

            RuleFor(x => x.Product.CategoryId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("must be greater than 0");

            RuleFor(x => x.Product.Price)
                .NotEmpty()
                .GreaterThan(0.01m).WithMessage("must be greater than 0");

            RuleFor(x => x.Product.ImageURL)
                .NotEmpty()
                .Matches(@"\.(jpg|jpeg|png|gif)$").WithMessage("use types .jpg, .jpeg, .png или .gif");


            RuleFor(x => x.Product.Amount)
                .NotEmpty()
                .GreaterThan(0).WithMessage("must be greater than 0")
                .InclusiveBetween(1, 200);
        }
    }
}
