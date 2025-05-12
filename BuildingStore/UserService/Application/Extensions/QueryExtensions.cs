namespace UserService.Application.Extensions
{
    /// <summary>
    /// Расширения для пагинации.
    /// </summary>
    public static class QueryExtensions
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int  page, int size)
        {
            return query.Skip((page -1) * size).Take(size);
        }
    }
}
