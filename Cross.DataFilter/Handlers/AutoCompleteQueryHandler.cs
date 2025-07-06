namespace Cross.DataFilter.Handlers;

public abstract class AutoCompleteQueryHandler<T, TFilter> : QueryHandler<T, AutoCompleteResult>
    where T : AutoCompleteQuery<TFilter>
{
    protected virtual bool OrderByName => true;

    public abstract IQueryable<NamedDto> GetQuery(TFilter filter);

    public AutoCompleteQueryHandler(ILogger<AutoCompleteQueryHandler<T, TFilter>> logger)
        : base(logger)
    {
    }

    protected override async Task<AutoCompleteResult> HandleAsync(T query, CancellationToken cancellationToken)
    {
        IQueryable<NamedDto> dbQuery = GetQuery(query.Filter);

        if (OrderByName)
        {
            dbQuery = dbQuery.ApplyOrdering();
        }

        if (query is { PageSize: not null, Page: not null })
        {
            dbQuery = dbQuery.Skip((query.Page.Value - 1) * query.PageSize.Value).Take(query.PageSize.Value + 1);
        }
        var result = new AutoCompleteResult
        {
            Data = await dbQuery.ToListAsync(cancellationToken),
            HasMore = false
        };
        if (result.Data.Count > query.PageSize)
        {
            result.HasMore = true;
            result.Data = result.Data.Take(query.PageSize.Value).ToList();
        }
        return result;
    }
}
