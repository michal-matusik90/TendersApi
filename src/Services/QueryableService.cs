using Microsoft.EntityFrameworkCore;
using TendersApi.Models;

namespace TendersApi.Services;

public sealed class QueryableService(IEnumerable<IQueryableSubservice> subservices)
{
    public IQueryable<T> Build<T>(DbSet<T> dbSet, SearchModel searchModel) where T : class
    {
        var query = dbSet.AsQueryable();

        var possibleSubservices = subservices.Where(x => x.CanHandle(searchModel));

        foreach (var subservice in possibleSubservices)
        {
            query = subservice.Handle(query, searchModel);
        }

        return query;
    }
}