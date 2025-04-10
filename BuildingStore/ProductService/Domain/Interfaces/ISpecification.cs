using System.Linq.Expressions;

namespace ProductService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс определения спецификации.
    /// </summary>
    public interface ISpecification<T> where T : class
    {
        /// <summary>
        /// Спецификация.
        /// </summary>
        Expression<Func<T, bool>> Criteria { get; }

        /// <summary>
        /// Сортировка по возрастанию.
        /// </summary>
        Expression<Func<T, object>> OrderBy { get; set; }

        /// <summary>
        /// Сортировка по убыванию.
        /// </summary>
        Expression<Func<T, object>> OrderByDescending { get; set; }

    }
}
