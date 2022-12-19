using CIS.Core.Results;
using CIS.InternalServices.DataAggregator.Configuration;
using DomainServices.ProductService.Clients;
using DomainServices.ProductService.Contracts;

namespace CIS.InternalServices.DataAggregator.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class ProductServiceWrapper : IServiceWrapper
{
    private readonly IProductServiceClient _productService;

    public ProductServiceWrapper(IProductServiceClient productService)
    {
        _productService = productService;
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        if (!input.CaseId.HasValue)
            throw new ArgumentNullException(nameof(InputParameters.CaseId));

        var result = await _productService.GetMortgage(input.CaseId.Value, cancellationToken);

        data.Mortgage = ServiceCallResult.ResolveAndThrowIfError<GetMortgageResponse>(result).Mortgage;
    }
}