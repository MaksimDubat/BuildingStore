using ProductService.Domain.Enums;

namespace ProductService.Application.DTOs
{
    /// <summary>
    /// DTO для вычисления рекомендованных параметров.
    /// </summary>
    public record CalculationDto(CategoryType CategoryKey, string Description, int Amount);
}
