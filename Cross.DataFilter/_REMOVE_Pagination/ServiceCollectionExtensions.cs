// namespace Cross.DataFilter._REMOVE_Pagination;
//
// public static class ServiceCollectionExtensions
// {
//     public static IServiceCollection AddDataFilter<TDbContext>(this IServiceCollection services, params Assembly[] assemblies)
//         where TDbContext : DbContext
//     {
//         // Filters
//         services.Scan(scan => scan
//             .FromAssemblies(assemblies.Distinct())
//             .AddClasses(classes => classes.AssignableTo(typeof(IQueryableFilter<,>)))
//             .AsImplementedInterfaces()
//             .WithScopedLifetime());
//
//         return services;
//     }
// }
