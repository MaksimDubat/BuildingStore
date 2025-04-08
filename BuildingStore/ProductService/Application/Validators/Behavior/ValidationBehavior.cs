using FluentValidation;
using MediatR;

namespace ProductService.Application.Validators.Behavior
{
    /// <summary>
    /// Проверка запроса на валидацию.
    /// </summary>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validator = validators.FirstOrDefault();
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validator != null)
            {
                var result = await _validator.ValidateAsync(request, cancellationToken);
                if (!result.IsValid)
                {
                    throw new ValidationException(result.Errors.First().ErrorMessage);
                }
            }

            return await next();
        }
    }
}
