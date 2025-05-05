using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Extensions;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    public class GetProductsFromFormQueryHandler : IRequestHandler<GetProductsFromFormQuery, RecomendationsDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsFromFormQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RecomendationsDto> Handle(GetProductsFromFormQuery request, CancellationToken cancellationToken)
        {
            if (request.Form.BathRoom <= 0 || request.Form.ToiletRoom <= 0 || request.Form.FloorHeatingRooms <= 0 || request.Form.AmoutnOfTaps <= 0 || request.Form.AmountOfWashingMachines <= 0 ||
                request.Form.AmountOfDishWashinfMachines <= 0 || request.Form.AmountOfSewers <= 0 || request.Form.TotalArea <= 0)
            {
                throw new ArgumentException("Wrong number");
            }

            var calculations = new Dictionary<int, CalculationDto>
            {
                {1, new CalculationDto (CategoryType.Baths,CategoryType.Baths.GetDescription(), request.Form.BathRoom ) },
                {2, new CalculationDto (CategoryType.Screws, CategoryType.Screws.GetDescription(), request.Form.BathRoom * 15 ) },
                {3, new CalculationDto (CategoryType.Toilets, CategoryType.Toilets.GetDescription(), request.Form.ToiletRoom ) },
                {4, new CalculationDto (CategoryType.Screws, CategoryType.Screws.GetDescription(), request.Form.ToiletRoom * 15 ) },
                {5, new CalculationDto (CategoryType.FloorHeating, CategoryType.FloorHeating.GetDescription(), request.Form.FloorHeatingRooms * 15 ) },
                {6, new CalculationDto (CategoryType.BathTaps, CategoryType.BathTaps.GetDescription(), request.Form.AmoutnOfTaps ) },
                {7, new CalculationDto (CategoryType.Screws, CategoryType.Screws.GetDescription(), request.Form.AmoutnOfTaps * 5 ) },
                {8, new CalculationDto (CategoryType.Pipes, CategoryType.Pipes.GetDescription(), request.Form.AmountOfWashingMachines * 10) },
                {9, new CalculationDto (CategoryType.Screws, CategoryType.Screws.GetDescription(), request.Form.AmountOfWashingMachines * 5 ) },
                {10, new CalculationDto (CategoryType.Pipes, CategoryType.Pipes.GetDescription(), request.Form.AmountOfWashingMachines * 15 ) },
                {11, new CalculationDto (CategoryType.Screws, CategoryType.Screws.GetDescription(), request.Form.AmountOfWashingMachines * 15 ) },

            };

            var products = await _unitOfWork.Products.GetAllAsync(cancellationToken);

            if (products == null)
            {
                throw new KeyNotFoundException("Products not found");
            }

            var matchedProducts = products
                .Where(p => calculations.Values.Any(c => (int)c.CategoryKey == p.CategoryId) && p.Amount > 0)
                .OrderBy(p => p.Price)
                .ToList();

            var productDto = _mapper.Map<IEnumerable<ProductResponseDto>>(matchedProducts);

            return new RecomendationsDto
            {
                Calculations = calculations,
                RecommendedProducts = productDto
            };

        }
    }
}
