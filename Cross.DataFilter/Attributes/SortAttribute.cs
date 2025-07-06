namespace Cross.DataFilter.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SortAttribute : Attribute
{
    public SortDirectionEnum Direction { get; }

    public int Order { get; }

    public SortAttribute(SortDirectionEnum direction = SortDirectionEnum.Asc)
        : this(0, direction)
    {
    }

    public SortAttribute(int order, SortDirectionEnum direction = SortDirectionEnum.Asc)
    {
        Direction = direction;
        Order = order;
    }
}
