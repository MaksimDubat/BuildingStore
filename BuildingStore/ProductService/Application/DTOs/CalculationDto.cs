namespace ProductService.Application.DTOs
{
    /// <summary>
    /// DTO для вычисления рекомендованных параметров.
    /// </summary>
    public record CalculationDto(int CategoryKey, string Description, int Amount);
}
