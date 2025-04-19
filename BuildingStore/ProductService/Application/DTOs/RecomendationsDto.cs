namespace ProductService.Application.DTOs
{
    /// <summary>
    /// DTO для рекомендаций.
    /// </summary>
    public class RecomendationsDto
    {
        /// <summary>
        /// Словарь с вычислениями.
        /// </summary>
        public Dictionary<int, CalculationDto> Calculations { get; set; }

        /// <summary>
        /// Рекомендованные товары.
        /// </summary>
        public IEnumerable<ProductDto> RecommendedProducts { get; set; }
    }
}
