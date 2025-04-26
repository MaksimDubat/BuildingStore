using ProductService.Application.DTOs;

namespace ProductService.Application.Models.RequestModels
{
    /// <summary>
    /// Модель добавления или обновления продукта.
    /// </summary>
    public class AddOrUpdateProductRequestModel
    {
        /// <summary>
        /// Наименование продукта.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Описание продукта.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Категория.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Цена продукта.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Изображение.
        /// </summary>
        public IFormFile Image { get; set; }

        /// <summary>
        /// Количество.
        /// </summary>
        public int Amount { get; set; }

    }
}
