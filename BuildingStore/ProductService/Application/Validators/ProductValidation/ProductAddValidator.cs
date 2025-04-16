using FluentValidation;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;
using ProductService.Application.Validators.CustomValidatorRules;

namespace ProductService.Application.Validators.ProductValidation
{
    /// <summary>
    /// Валидация добавления продукта.
    /// </summary>
    public class ProductAddValidator : AbstractValidator<AddProductCommand>
    {
        public ProductAddValidator()
        {
            RuleFor(x => x.Product.Name).ValidString();

            RuleFor(x => x.Product.Description).ValidString();

            RuleFor(x => x.Product.CategoryId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("must be greater than 0");

            RuleFor(x => x.Product.Price)
                .NotEmpty()
                .GreaterThan(0.01m).WithMessage("must be greater than 0");

            RuleFor(x => x.Product.ImageURL)
                .NotEmpty()
                .Matches(@"\.(jpg|jpeg|png|gif)$").WithMessage("use types .jpg, .jpeg, .png or .gif");


            RuleFor(x => x.Product.Amount)
                .NotEmpty()
                .GreaterThan(0).WithMessage("must be greater than 0")
                .InclusiveBetween(1, 200);

        }
    }
}
