using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;
using ProductService.Domain.Entities;
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
            if (request.BathRoom <= 0 || request.ToiletRoom <= 0 || request.FloorHeatingRooms <= 0 || request.AmoutnOfTaps <= 0 || request.AmountOfWashingMachines <= 0 ||
                request.AmountOfDishWashinfMachines <= 0 || request.AmountOfSewers <= 0 || request.TotalArea <= 0)
            {
                throw new ArgumentException("Wrong number");
            }

            var calculations = new Dictionary<int, CalculationDto>
            {
                {1, new CalculationDto (8, "Amount of baths", request.BathRoom ) },
                {2, new CalculationDto (4, "Amount of screws for baths", request.BathRoom * 15 ) },
                {3, new CalculationDto (9, "Amount of toilets", request.ToiletRoom ) },
                {4, new CalculationDto (4, "Amount of screws for baths", request.ToiletRoom * 15 ) },
                {5, new CalculationDto (6, "Amount of screws for baths", request.FloorHeatingRooms * 15 ) },
                {6, new CalculationDto (12, "Amount of taps",   request.AmoutnOfTaps ) },
                {7, new CalculationDto (7, "Amount of screws for baths", request.AmoutnOfTaps * 5 ) },
                {8, new CalculationDto (4, "Amount of washingmachines", request.AmountOfWashingMachines ) },
                {9, new CalculationDto (4, "Amount of screws for washingmachine", request.AmountOfWashingMachines * 5 ) },
                {10, new CalculationDto (4, "Amount of screws for washingmachine", request.AmountOfWashingMachines * 15 ) },
                {11, new CalculationDto (4, "Amount of screws for washingmachine", request.AmountOfWashingMachines * 15 ) },

            };

            var products = await _unitOfWork.Products.GetAllAsync(cancellationToken);

            if (products == null)
            {
                throw new KeyNotFoundException("Products not found");
            }

            var matchedProducts = products
                .Where(p => calculations.Values.Any(c => c.CategoryKey == p.CategoryId) && p.Amount > 0)
                .OrderBy(p => p.Price)
                .ToList();

            var productDto = _mapper.Map<IEnumerable<ProductDto>>(matchedProducts);

            return new RecomendationsDto
            {
                Calculations = calculations,
                RecommendedProducts = productDto
            };

        }
    }
}
