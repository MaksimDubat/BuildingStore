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
            if(request.BathRoom <= 0 || request.ToiletRoom <= 0 || request.FloorHeatingRooms <=0 || request.AmoutnOfTaps <=0 || request.AmountOfWashingMachines <= 0 ||
                request.AmountOfDishWashinfMachines <= 0 || request.AmountOfSewers <= 0 || request.TotalArea <= 0)
            {
                throw new ArgumentException("Wrong number");
            }

            var calculations = new Dictionary<int, CalculationDto>()
            {
                {1, new CalculationDto {CategoryKey = 8, Description = "Amount of baths", Amount = request.BathRoom } },
                {2, new CalculationDto {CategoryKey = 4, Description = "Amount of screws for baths", Amount = request.BathRoom * 15 } },
                {3, new CalculationDto {CategoryKey = 9, Description = "Amount of toilets", Amount = request.ToiletRoom } },
                {4, new CalculationDto {CategoryKey = 4, Description = "Amount of screws for baths", Amount = request.ToiletRoom * 15 } },
                {5, new CalculationDto {CategoryKey = 6, Description = "Amount of screws for baths", Amount = request.FloorHeatingRooms * 15 } },
                {6, new CalculationDto {CategoryKey = 12, Description = "Amount of taps", Amount = request.AmoutnOfTaps } },
                {7, new CalculationDto {CategoryKey = 7, Description = "Amount of screws for baths", Amount = request.AmoutnOfTaps * 5 } },
                {8, new CalculationDto {CategoryKey = 4, Description = "Amount of washingmachines", Amount = request.AmountOfWashingMachines } },
                {9, new CalculationDto {CategoryKey = 4, Description = "Amount of screws for washingmachine", Amount = request.AmountOfWashingMachines * 5 } },
                {10, new CalculationDto {CategoryKey = 4, Description = "Amount of screws for washingmachine", Amount = request.AmountOfWashingMachines * 15 } },
                {11, new CalculationDto {CategoryKey = 4, Description = "Amount of screws for washingmachine", Amount = request.AmountOfWashingMachines * 15 } },

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
