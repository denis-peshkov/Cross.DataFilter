namespace Cross.DataFilter.Handlers;

public abstract class PaginatedItemsQuery<TResult> : Query<PaginatedResult<TResult>>, IHasPaging
    where TResult : class
{
    public int? Page { get; }

    public int? PageSize { get; }

    public IReadOnlyCollection<SortingDto>? Sorting { get; }

    protected PaginatedItemsQuery(int? page, int? pageSize, IReadOnlyCollection<SortingDto>? sorting)
    {
        Page = page;
        PageSize = pageSize;
        Sorting = sorting;
    }
}

public abstract class PaginatedItemsQuery<TFilter, TResult> : PaginatedItemsQuery<TResult>
    where TFilter : class
    where TResult : class
{
    public TFilter? Filter { get; }

    protected PaginatedItemsQuery(int? page, int? pageSize, IReadOnlyCollection<SortingDto>? sorting, TFilter? filter)
        : base(page, pageSize, sorting)
    {
        Filter = filter;
    }
}
