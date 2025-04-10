namespace ProductService.Application.DTOs
{
    /// <summary>
    /// DTO для вычисления рекомендованных параметров.
    /// </summary>
    public class CalculationDto
    {
        /// <summary>
        /// Категория.
        /// </summary>
        public int CategoryKey { get; set; }    

        /// <summary>
        /// Описание.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Количество.
        /// </summary>
        public int Amount { get; set; }
    }
}
