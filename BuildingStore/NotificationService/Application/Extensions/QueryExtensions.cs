using MongoDB.Driver;

namespace NotificationService.Application.Extensions
{
    /// <summary>
    /// Расширения для пагинации.
    /// </summary>
    public static class QueryExtensions
    {
        public static IFindFluent<T, T> ApplyPagination<T>(this IFindFluent<T, T> query, int page, int size)
        {
            return query.Skip((page - 1) * size).Limit(size);
        }
    }
}
