namespace Cross.DataFilter.Dtos;

public abstract class SortedResult<TDto>
    where TDto : class
{
    public IReadOnlyCollection<TDto> Data { get; }

    protected SortedResult(IReadOnlyCollection<TDto> data)
    {
        Data = data;
    }
}
