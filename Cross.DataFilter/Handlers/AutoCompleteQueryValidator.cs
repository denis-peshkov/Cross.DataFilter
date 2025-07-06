namespace Cross.DataFilter.Handlers;

public class AutoCompleteQueryValidator<T> : AbstractValidator<T>
    where T : AutoCompleteQuery
{
    public AutoCompleteQueryValidator()
    {
        When(x => x.PageSize.HasValue, () =>
        {
            RuleFor(p => p.PageSize).InclusiveBetween(1, 500);
            RuleFor(p => p.Page).NotNull().GreaterThan(0);
        });
    }
}
