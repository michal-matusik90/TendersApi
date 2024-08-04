using TendersApi.Models;

namespace TendersApi.Services;

public sealed class SkipQueryableSubservice : IQueryableSubservice
{
    public bool CanHandle(SearchModelRequest searchModel)
        => searchModel.Skip > 0;

    public IQueryable<T> Handle<T>(IQueryable<T> query, SearchModelRequest searchModel)
    {
        return query.Skip(searchModel.Skip);
    }
}
