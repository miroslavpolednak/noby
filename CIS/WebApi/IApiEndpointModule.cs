namespace CIS.Infrastructure.WebApi;

public interface IApiEndpointModule
{
    void Register(IEndpointRouteBuilder builder);
}
