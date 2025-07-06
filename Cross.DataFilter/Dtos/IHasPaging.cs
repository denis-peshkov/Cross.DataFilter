namespace Cross.DataFilter.Dtos;

public interface IHasPaging
{
    int? Page { get; }

    int? PageSize { get; }
}
