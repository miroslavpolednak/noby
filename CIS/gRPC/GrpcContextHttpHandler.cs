namespace CIS.Infrastructure.gRPC;

internal sealed class GrpcContextHttpHandler : DelegatingHandler
{
    private readonly int? _currentUserId;

    public GrpcContextHttpHandler(HttpMessageHandler innerHandler, int? currentUserId)
        : base(innerHandler)
    {
        _currentUserId = currentUserId;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_currentUserId.HasValue)
#pragma warning disable CA1305 // Specify IFormatProvider
            request.Headers.Add(Core.Security.Constants.ContextUserHttpHeaderKey, _currentUserId.Value.ToString());
#pragma warning restore CA1305 // Specify IFormatProvider

        return base.SendAsync(request, cancellationToken);
    }
}
