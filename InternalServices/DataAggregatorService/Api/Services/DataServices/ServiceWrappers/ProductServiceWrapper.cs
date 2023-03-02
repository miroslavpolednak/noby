using DomainServices.ProductService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class ProductServiceWrapper : IServiceWrapper
{
    private readonly IProductServiceClient _productService;

    public ProductServiceWrapper(IProductServiceClient productService)
    {
        _productService = productService;
    }

    public DataSource DataSource => DataSource.ProductService;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateCaseId();

        var result = await _productService.GetMortgage(input.CaseId!.Value, cancellationToken);

        data.Mortgage = result.Mortgage;
    }
}