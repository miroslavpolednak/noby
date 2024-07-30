namespace DomainServices.ProductService.ExternalServices.Pcp.V1;

internal sealed class MockPcpClient
    : IPcpClient
{
    public Task<ProductInstance3[]> GetByOtherIds(long caseId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Array.Empty<ProductInstance3>());
    }

    public Task<string> CreateProduct(long caseId, long customerKbId, string pcpProductId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult("nejake id");
    }

	public Task<string> UpdateProduct(string pcpId, long customerKbId, CancellationToken cancellationToken = default)
	{
        return Task.FromResult("nejake id");
	}
}
