namespace Cross.DataFilter.Handlers;

public class PaginatedItemsQueryValidator : AbstractValidator<IHasPaging>
{
    public PaginatedItemsQueryValidator()
    {
        RuleFor(x => x.Page)
            .NotNull().When(x => x.PageSize.HasValue)
            .InclusiveBetween(0, int.MaxValue).When(x => x.Page.HasValue);

        RuleFor(x => x.PageSize)
            .NotNull().When(x => x.Page.HasValue)
            .InclusiveBetween(0, int.MaxValue).When(x => x.Page.HasValue);
    }
}
