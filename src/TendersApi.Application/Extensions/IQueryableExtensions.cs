using TendersApi.Application.Models;

namespace TendersApi.Application.Extensions;

public static class IQueryableExtensions
{
    public static PaginatedReadOnlyCollection<T> ToPaginatedList<T>(this IQueryable<T> queryable, int page, int pageSize)
        => PaginatedReadOnlyCollection<T>.Create(queryable, page, pageSize);

    public static Task<PaginatedReadOnlyCollection<T>> ToPaginatedListAsync<T>(this IQueryable<T> queryable, int page, int pageSize, CancellationToken cancellationToken = default)
        => PaginatedReadOnlyCollection<T>.CreateAsync(queryable, page, pageSize, cancellationToken);
}
