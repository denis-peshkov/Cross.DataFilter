namespace Cross.DataFilter.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class RequiredSortAttribute : Attribute
{
    public SortDirectionEnum Direction { get; }

    public int Order { get; }

    public RequiredSortAttribute(SortDirectionEnum direction = SortDirectionEnum.Asc)
        : this(0, direction)
    {
    }

    public RequiredSortAttribute(int order, SortDirectionEnum direction = SortDirectionEnum.Desc)
    {
        Direction = direction;
        Order = order;
    }
}
