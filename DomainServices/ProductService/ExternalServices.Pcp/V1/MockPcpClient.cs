namespace DomainServices.ProductService.ExternalServices.Pcp.V1;

internal sealed class MockPcpClient
    : IPcpClient
{
    public Task CreateProduct(long caseId, long customerKbId, string pcpProductId, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
