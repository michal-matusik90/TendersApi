using TendersApi.Models;

namespace TendersApi.Services;

public interface IQueryableSubservice
{
    public bool CanHandle(SearchModelRequest searchModel);
    public IQueryable<T> Handle<T>(IQueryable<T> query, SearchModelRequest searchModel);
}

