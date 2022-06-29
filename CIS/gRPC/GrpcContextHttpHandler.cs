namespace CIS.Infrastructure.gRPC;

internal sealed class GrpcContextHttpHandler : DelegatingHandler
{
    public GrpcContextHttpHandler(HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("userid-test2", "111");
        return base.SendAsync(request, cancellationToken);
    }
}
