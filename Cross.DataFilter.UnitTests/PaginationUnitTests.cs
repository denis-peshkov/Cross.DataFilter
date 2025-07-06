namespace Cross.DataFilter.UnitTests;

[TestFixture]
public class PaginationTests
{
    private TestDbContext _dbContext;

    private ILogger<TestEntityPaginationQueryHandler> logger;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        _dbContext = new TestDbContext(options);

        // Seed test data
        _dbContext.TestEntities.AddRange(new[]
        {
            new TestEntity { Id = 1, Name = "Test 1" },
            new TestEntity { Id = 2, Name = "Test 2" },
            new TestEntity { Id = 3, Name = "Test 3" },
            new TestEntity { Id = 4, Name = "Test 4" },
            new TestEntity { Id = 5, Name = "Test 5" }
        });

        _dbContext.SaveChanges();

        logger = Mock.Of<ILogger<TestEntityPaginationQueryHandler>>();
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public void PaginationRequest_ValidProperties()
    {
        // Arrange
        var query = new TestEntityPaginationQuery(
            2,
            8,
            new[]
            {
                new SortingDto { SortColumnName = "Name", SortDirection = SortDirectionEnum.Asc }
            },
            null);

        // Assert
        Assert.That(query.Page, Is.EqualTo(2));
        Assert.That(query.PageSize, Is.EqualTo(8));
        Assert.That(query.Sorting.First().SortColumnName, Is.EqualTo("Name"));
        Assert.That(query.Sorting.First().SortDirection, Is.EqualTo(SortDirectionEnum.Asc));
    }

    [Test]
    public void PaginationResult_ValidProperties()
    {
        // Arrange
        var items = new List<TestEntity>
        {
            new TestEntity { Id = 1, Name = "Test" }
        };

        var result = new PaginatedResult<TestEntity>(null, null, 1, items);

        // Assert
        Assert.That(result.Data, Is.EqualTo(items));
        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task PaginationQueryHandler_HandleAsync_ReturnsPaginatedResult()
    {
        // Arrange
        var query = new TestEntityPaginationQuery(
            1,
            2,
            new[]
            {
                new SortingDto { SortColumnName = "Name", SortDirection = SortDirectionEnum.Asc }
            },
            null);

        var handler = new TestEntityPaginationQueryHandler(logger, _dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Data.Count, Is.EqualTo(2));
        Assert.That(result.Count, Is.EqualTo(5));
    }

    [Test]
    public async Task PaginationQueryHandler_HandleAsync_WithInvalidSkip_ReturnsEmptyResult()
    {
        // Arrange
        var query = new TestEntityPaginationQuery(
            10,
            999,
            new[]
            {
                new SortingDto { SortColumnName = "Name", SortDirection = SortDirectionEnum.Asc }
            },
            null);

        var handler = new TestEntityPaginationQueryHandler(logger, _dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Data, Is.Empty);
        Assert.That(result.Count, Is.EqualTo(5));
    }

    [Test]
    public async Task PaginationQueryHandler_HandleAsync_WithDescendingSort()
    {
        // Arrange
        var query = new TestEntityPaginationQuery(
            1,
            2,
            new[]
            {
                new SortingDto { SortColumnName = "Name", SortDirection = SortDirectionEnum.Desc }
            },
            null);

        var handler = new TestEntityPaginationQueryHandler(logger, _dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Data.First().Name, Is.EqualTo("Test 5"));
        Assert.That(result.Data.Last().Name, Is.EqualTo("Test 4"));
    }
}
