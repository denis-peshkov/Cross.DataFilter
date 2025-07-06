namespace Cross.DataFilter.Extensions;

public static class PaginatedExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IOrderedQueryable<T> source, int? page, int? pageSize)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(source);

        if (page.HasValue && pageSize.HasValue)
        {
            return source.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return source;
    }

    public static IEnumerable<T> ApplyPaging<T>(this IEnumerable<T> source, int? page, int? pageSize)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(source);

        if (page.HasValue && pageSize.HasValue)
        {
            return source.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return source;
    }

    public static async Task<PaginatedResult<T>> PaginateAsync<T>(
        this IQueryable<T> source,
        int? page,
        int? pageSize,
        IReadOnlyCollection<SortingDto>? sorting,
        CancellationToken cancellationToken)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(source);

        var data = await source
            .ApplyOrdering(sorting)
            .ApplyPaging(page, pageSize)
            .ToListAsync(cancellationToken);

        var count = await source.CountAsync(cancellationToken);

        return new PaginatedResult<T>(page, pageSize, count, data);
    }

    public static async Task<PaginatedResult<TResult>> PaginateAsync<T, TResult>(
        this IQueryable<T> source,
        int? page,
        int? pageSize,
        Expression<Func<T, TResult>> select,
        IReadOnlyCollection<SortingDto>? sorting,
        CancellationToken cancellationToken)
        where T : class
        where TResult : class
    {
        var dataFiltered = source
            .Select(select);

        return await dataFiltered.PaginateAsync(page, pageSize, sorting, cancellationToken);
    }
}
