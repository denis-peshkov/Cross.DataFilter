namespace Cross.DataFilter.Dtos;

public class AutoCompleteResult<TDto>
    where TDto : class
{
    public IReadOnlyCollection<TDto> Data { get; set; }

    public bool HasMore { get; set; }
}

public class AutoCompleteResult : AutoCompleteResult<NamedDto>
{
}
