using Microsoft.EntityFrameworkCore;

namespace TendersApi.Application.Models;

public sealed class PaginatedReadOnlyCollection<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PaginatedReadOnlyCollection(IReadOnlyCollection<T> items, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(items.Count / (double)pageSize);
        TotalCount = items.Count;
        Items = items;
    }

    public static PaginatedReadOnlyCollection<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var items = source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToArray();

        return new PaginatedReadOnlyCollection<T>(items, pageNumber, pageSize);
    }

    public static async Task<PaginatedReadOnlyCollection<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return new PaginatedReadOnlyCollection<T>(items, pageNumber, pageSize);
    }
}
