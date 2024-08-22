namespace TendersApi.Application.Interfaces;

public interface ITendersService
{
    Task<HttpResponseMessage> GetTenders(int page, CancellationToken cancellationToken);
}
