namespace Cross.DataFilter.Dtos;

public class AutoCompleteItemDto<TKey, TValue>
{
    public string GroupName { get; set; }

    public bool Status { get; set; }

    public TKey Key { get; set; }

    public TValue Value { get; set; }
}
