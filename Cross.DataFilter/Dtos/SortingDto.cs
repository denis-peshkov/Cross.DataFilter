namespace Cross.DataFilter.Dtos;

public class SortingDto
{
    /// <summary>
    /// Название поля/колонки, по которой нужно сортировать
    /// </summary>
    public string SortColumnName { get; init; }

    /// <summary>
    /// Поддерживает два значения: Asc, Desc
    /// </summary>
    public SortDirectionEnum? SortDirection { get; init; }
}
