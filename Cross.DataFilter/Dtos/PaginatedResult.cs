namespace Cross.DataFilter.Dtos;

public class PaginatedResult<TDto> : SortedResult<TDto>
    where TDto : class
{
    public int? Page { get; }

    public int? PageSize { get; }

    public long Count { get; }

    public PaginatedResult(int? page, int? pageSize, long count, IReadOnlyCollection<TDto> data) : base(data)
    {
        Page = page;
        PageSize = pageSize;
        Count = count;
    }
}
