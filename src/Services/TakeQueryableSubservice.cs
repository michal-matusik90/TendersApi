using TendersApi.Models;

namespace TendersApi.Services;

public sealed class TakeQueryableSubservice : IQueryableSubservice
{
    public bool CanHandle(SearchModelRequest searchModel)
        => searchModel.Take > 0;

    public IQueryable<T> Handle<T>(IQueryable<T> query, SearchModelRequest searchModel)
    {
        return query.Take(searchModel.Take);
    }
}

