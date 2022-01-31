namespace CIS.Infrastructure.gRPC;

public sealed class GrpcServiceUriSettings<TService>
    where TService : class
{
    public Type ServiceType { get; init; }

    public Uri Url { get; init; }

    public bool IsInvalidCertificateAllowed { get; init; }

    public GrpcServiceUriSettings(string serviceUrl, bool isInvalidCertificateAllowed)
    {
        if (string.IsNullOrEmpty(serviceUrl))
            throw new Core.Exceptions.CisArgumentNullException(12, "Service URL is empty or null", nameof(serviceUrl));

        ServiceType = typeof(TService);
        IsInvalidCertificateAllowed = isInvalidCertificateAllowed;
        Url = new Uri(serviceUrl);
    }
}
