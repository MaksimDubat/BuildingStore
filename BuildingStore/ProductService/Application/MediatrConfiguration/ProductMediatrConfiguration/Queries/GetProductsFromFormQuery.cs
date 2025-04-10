using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение рекомендованных товаров исходя из формы.
    /// </summary>
    public record GetProductsFromFormQuery(
        int BathRoom, /// Количество ванных комнат.
        int ToiletRoom, /// Количество туалетных комнат.
        int FloorHeatingRooms, /// Количество комнат с теплым полом.
        int AmoutnOfTaps, /// Количество кранов.
        int AmountOfWashingMachines, /// Количество стиральных машин.
        int AmountOfDishWashinfMachines, /// Количество посудомоечных машин.
        int AmountOfSewers, /// Количество канализаций (квартира 1, дом >1).
        double TotalArea /// Общая площадь.
            ) : IRequest<RecomendationsDto>;

}
