namespace Cross.DataFilter.Tests;

public sealed record TestEntityPaginationQuery : PaginatedItemsQuery<TestEntityFilter, TestEntity>
{
    public TestEntityPaginationQuery(int? page, int? pageSize, IReadOnlyCollection<SortingDto>? sorting, TestEntityFilter? filter)
        : base(page, pageSize, sorting, filter)
    {
    }
}
