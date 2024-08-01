using TendersApi.Models;

namespace TendersApi.Services;

public interface IQueryableSubservice
{
    public bool CanHandle(SearchModel searchModel);
    public IQueryable<T> Handle<T>(IQueryable<T> query, SearchModel searchModel);
}

