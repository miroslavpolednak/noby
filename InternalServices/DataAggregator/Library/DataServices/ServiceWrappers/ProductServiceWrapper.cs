using CIS.Core.Results;
using DomainServices.ProductService.Clients;
using DomainServices.ProductService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices.ServiceWrappers;

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

        //TODO: Mock
        data.Mortgage.PaymentAccount = new PaymentAccount
        {
            Prefix = "001",
            Number = "123456789",
            BankCode = "0000"
        };
    }
}