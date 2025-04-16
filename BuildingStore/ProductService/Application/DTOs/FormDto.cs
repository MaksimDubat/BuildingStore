namespace ProductService.Application.DTOs
{
    /// <summary>
    /// DTO для заполнения формы.
    /// </summary>
    public class FormDto
    {
        /// <summary>
        /// Количество ванных комнат.
        /// </summary>
        public int BathRoom {  get; set; }

        /// <summary>
        /// Количество туалетных комант.
        /// </summary>
        public int ToiletRoom {  get; set; }

        /// <summary>
        /// Количество комнат с подогревом пола.
        /// </summary>
        public int FloorHeatingRooms { get; set; }

        /// <summary>
        /// Количество кранов.
        /// </summary>
        public int AmoutnOfTaps {  get; set; } 

        /// <summary>
        /// Количество стиральных машин.
        /// </summary>
        public int AmountOfWashingMachines {  get; set; }

        /// <summary>
        /// Количество посудомоечных машин.
        /// </summary>
        public int AmountOfDishWashinfMachines {  get; set; }

        /// <summary>
        /// Количество канализаций.
        /// </summary>
        public int AmountOfSewers {  get; set; }

        /// <summary>
        /// Общая площать
        /// </summary>
        public double TotalArea { get; set; }
    }
}
