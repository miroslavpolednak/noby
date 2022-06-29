namespace CIS.Infrastructure.gRPC;

internal sealed class GrpcContextHttpHandler : DelegatingHandler
{
    private readonly Core.Security.ICurrentUserAccessor _userAccessor;

    public GrpcContextHttpHandler(HttpMessageHandler innerHandler, Core.Security.ICurrentUserAccessor currentUserAccessor)
        : base(innerHandler)
    {
        _userAccessor = currentUserAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_userAccessor.IsAuthenticated)
#pragma warning disable CA1305 // Specify IFormatProvider
            request.Headers.Add(Core.Security.Constants.ContextUserHttpHeaderKey, _userAccessor.User!.Id.ToString());
#pragma warning restore CA1305 // Specify IFormatProvider

        return base.SendAsync(request, cancellationToken);
    }
}
