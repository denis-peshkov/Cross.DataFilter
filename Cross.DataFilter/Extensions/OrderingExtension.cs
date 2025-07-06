namespace Cross.DataFilter.Extensions;

public static class OrderingExtension
{
    public static IOrderedQueryable<TEntity> ApplyOrdering<TEntity>(this IEnumerable<TEntity> source)
        => source.ApplyOrdering(Array.Empty<SortingDto>());

    public static IOrderedQueryable<TEntity> ApplyOrdering<TEntity>(this IEnumerable<TEntity> source, IReadOnlyCollection<SortingDto>? sorting)
        => source.AsQueryable().ApplyOrdering(sorting);

    public static IOrderedQueryable<TEntity> ApplyOrdering<TEntity>(this IQueryable<TEntity> source)
        => source.ApplyOrdering(Array.Empty<SortingDto>());

    public static IOrderedQueryable<TEntity> ApplyOrdering<TEntity>(this IQueryable<TEntity> source, IReadOnlyCollection<SortingDto>? sorting)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(sorting);

        var type = typeof(TEntity);
        var props = type.GetRuntimeProperties().ToList();
        if (sorting.Any())
        {
            foreach (var sort in sorting)
            {
                var sortProperty = GetProperty(props, sort.SortColumnName)
                                   ?? throw new ArgumentException($"Property {sort.SortColumnName} does not exist in {type.Name}");
                if (sortProperty.GetCustomAttribute<NoSortAttribute>() != null)
                {
                    throw new ArgumentException($"Sort on property {sort.SortColumnName} not allowed in {type.Name}");
                }
                source = source.AddOrder(sortProperty, sort.SortDirection ?? SortDirectionEnum.Asc);
            }
        }
        else
        {
            var data = props
                .Select(x => new { Property = x, Attr = x.GetCustomAttribute<SortAttribute>() })
                .Where(x => x.Attr != null)
                .OrderBy(x => x.Attr!.Order);
            foreach (var p in data)
            {
                source = source.AddOrder(p.Property, p.Attr!.Direction);
            }
        }

        var requredSorts = props
            .Where(x => sorting.Select(s => s.SortColumnName).Contains(x.Name.ToLowerInvariant()))
            .Where(x => x.GetCustomAttribute<RequiredSortAttribute>(false) != null)
            .Select(x => new { Property = x, Attr = x.GetCustomAttribute<RequiredSortAttribute>()! });

        foreach (var sort in requredSorts.OrderBy(x => x.Attr.Order))
        {
            source = source.AddOrder(sort.Property, sort.Attr.Direction);
        }

        return (IOrderedQueryable<TEntity>)source;
    }

    private static PropertyInfo GetProperty(IEnumerable<PropertyInfo> props, string sortPropertyName)
    {
        var names = sortPropertyName.Split('.', StringSplitOptions.RemoveEmptyEntries);

        PropertyInfo currentProp = null;
        var currentProps = props;

        foreach (var name in names)
        {
            currentProp = currentProps
                   .Where(x => string.Equals(x.Name, sortPropertyName, StringComparison.OrdinalIgnoreCase)
                               || string.Equals(x.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name, sortPropertyName, StringComparison.OrdinalIgnoreCase) // used for body
                               || string.Equals(x.GetCustomAttribute<BindPropertyAttribute>()?.Name, sortPropertyName, StringComparison.OrdinalIgnoreCase)) // used for query params
                   .FirstOrDefault()
            ?? throw new NotSupportedException();

            currentProps = currentProp.PropertyType.GetProperties();
        }

        return currentProp ?? throw new NotSupportedException();
    }

    private static IOrderedQueryable<T> AddOrder<T>(this IQueryable<T> query, PropertyInfo sortProperty, SortDirectionEnum sortDirectionEnum = SortDirectionEnum.Asc)
    {
        var names = sortProperty.GetCustomAttribute<SortByAttribute>()?.Names.ToList();
        if (names != null)
        {
            foreach (var singlName in names)
            {
                query = query.AddSingleOrder(singlName, sortDirectionEnum);
            }
            return (IOrderedQueryable<T>)query;
        }

        var name = sortProperty.Name;

        if (sortProperty.PropertyType.IsClass && sortProperty.PropertyType != typeof(string))
        {
            var props = sortProperty.PropertyType
                .GetRuntimeProperties()
                .Where(x => x.GetCustomAttributes<SortAttribute>().Any());

            names = props.Select(x => $"{name}.{x.Name}").ToList();

            foreach(var propName in names)
            {
                query = query.AddSingleOrder(propName, sortDirectionEnum);
            }
            return (IOrderedQueryable<T>)query;
        }

        return query.AddSingleOrder(name, sortDirectionEnum);
    }

    private static IOrderedQueryable<T> AddSingleOrder<T>(this IQueryable<T> query, string name, SortDirectionEnum sortDirectionEnum = SortDirectionEnum.Asc)
        => query.Expression.Type == typeof(IOrderedQueryable<T>)
            ? query.ThenBy(name, sortDirectionEnum == SortDirectionEnum.Desc)
            : query.OrderBy(name, sortDirectionEnum == SortDirectionEnum.Desc);

    private static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propName, bool desc = false)
        => SortBy(source, propName, desc ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy));

    private static IOrderedQueryable<TSource> ThenBy<TSource>(this IQueryable<TSource> source, string propName, bool desc = false)
        => SortBy(source, propName, desc ? nameof(Queryable.ThenByDescending) : nameof(Queryable.ThenBy));

    private static IOrderedQueryable<TSource> SortBy<TSource>(IQueryable<TSource> source, string propName, string operationName)
    {
        var type = typeof(TSource);
        var props = propName.Split('.', StringSplitOptions.RemoveEmptyEntries);

        var par = Expression.Parameter(type);
        PropertyInfo? prop = null;
        Expression exp = par;
        foreach (var name in props)
        {
            prop = exp.Type.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (prop != null)
            {
                exp = Expression.Property(exp, prop);
            }
        }

        if (prop != null)
        {
            if (prop.PropertyType == typeof(string))
            {
                var toLower = prop.PropertyType.GetMethod(nameof(string.ToLower), Array.Empty<Type>());
                if (toLower != null)
                {
                    exp = Expression.Call(exp, toLower);
                }
            }
            var l = Expression.Lambda(Expression.GetFuncType(type, prop.PropertyType), exp, par);
            var meth = typeof(Queryable).GetMethods()
                .Where(m => m.Name == operationName)
                .First();
            if (meth.MakeGenericMethod(type, prop.PropertyType).Invoke(null, new object[] { source, l }) is IOrderedQueryable<TSource> res)
            {
                return res;
            }
        }

        throw new NotSupportedException($"Sorting not possible by {propName}");
    }
}
