using ProductService.Domain.Entities;

namespace ProductService.Domain.Interfaces
{
    /// <summary>
    /// Репозиторий по работе с категориями.
    /// </summary>
    public interface ICategoryRepository : IBaseRepository<Category>
    {
    }
}
