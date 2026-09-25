namespace Cross.DataFilter.Tests;

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
    [Category(TestCategory.UNIT)]
    public void GivenValidPagingAndSorting_WhenCreateQuery_ThenPropertiesMatch()
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
    [Category(TestCategory.UNIT)]
    public void GivenItems_WhenCreatePaginatedResult_ThenPropertiesMatch()
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
    [Category(TestCategory.INTEGRATION)]
    public async Task GivenSeededEntities_WhenHandleFirstPage_ThenReturnsPaginatedResult()
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
    [Category(TestCategory.INTEGRATION)]
    public async Task GivenPageBeyondData_WhenHandle_ThenReturnsEmptyPageWithTotalCount()
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
    [Category(TestCategory.INTEGRATION)]
    public async Task GivenDescendingSort_WhenHandle_ThenReturnsSortedPage()
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
