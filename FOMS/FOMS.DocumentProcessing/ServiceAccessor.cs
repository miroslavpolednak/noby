using Microsoft.AspNetCore.Http;

namespace FOMS.DocumentProcessing;

internal sealed class ServiceAccessor
{
    private readonly IHttpContextAccessor _httpContext;

    public ServiceAccessor(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public TService GetRequiredService<TService>()
    {
        return (TService)(_httpContext.HttpContext?.RequestServices.GetService(typeof(TService)) ?? throw new Exception("Service not found or HttpContext is empty"));
    }
}
