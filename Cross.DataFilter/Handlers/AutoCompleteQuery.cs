namespace Cross.DataFilter.Handlers;

public abstract record AutoCompleteQuery : Query<AutoCompleteResult>
{
    public int? Page { get; }

    public int? PageSize { get; }

    protected AutoCompleteQuery(int? page, int? pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }
}

public abstract record AutoCompleteQuery<TFilter> : AutoCompleteQuery
{
    public TFilter Filter { get; }

    protected AutoCompleteQuery(int? page, int? pageSize, TFilter filter)
        : base(page, pageSize)
    {
        Filter = filter;
    }
}
