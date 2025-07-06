namespace Cross.DataFilter.UnitTests;

public class TestEntityPaginationQueryHandler : QueryHandler<TestEntityPaginationQuery, PaginatedResult<TestEntity>>
{
    private readonly TestDbContext _dbContext;

    public TestEntityPaginationQueryHandler(ILogger<TestEntityPaginationQueryHandler> logger, TestDbContext dbContext)
        : base(logger)
    {
        _dbContext = dbContext;
    }

    protected override async Task<PaginatedResult<TestEntity>> HandleAsync(TestEntityPaginationQuery query, CancellationToken cancellationToken)
    {
        var testEntities = _dbContext.TestEntities
            .AsNoTracking();

        if (query.Filter?.Id != null)
        {
            testEntities = testEntities.Where(x => x.Id == query.Filter.Id);
        }

        if (query.Filter?.Name != null)
        {
            testEntities = testEntities.Where(x => x.Name == query.Filter.Name);
        }

        var roleItemDtos = await testEntities
            .PaginateAsync(query.Page, query.PageSize, query.Sorting, cancellationToken);

        return roleItemDtos;
    }
}
