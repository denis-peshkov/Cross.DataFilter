namespace Cross.DataFilter.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SortByAttribute : Attribute
{
    public string[] Names { get; }

    public SortByAttribute(params string[] names)
    {
        Names = names;
    }
}
