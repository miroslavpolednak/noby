using DomainServices.CaseService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.CodebookService.Clients;
using NOBY.Services.Customer;

namespace NOBY.Api.Endpoints.Customer.GetCustomerDetailWithChanges;

internal sealed class GetCustomerDetailWithChangesHandler : IRequestHandler<GetCustomerDetailWithChangesRequest, CustomerGetCustomerDetailWithChangesResponse>
{
    public async Task<CustomerGetCustomerDetailWithChangesResponse> Handle(GetCustomerDetailWithChangesRequest request, CancellationToken cancellationToken)
    {
        var customerInfo = await _changedDataService.GetCustomerWithChangedData(request.CustomerOnSAId, cancellationToken);

        // SA instance
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(customerInfo.CustomerOnSA.SalesArrangementId, cancellationToken);

        await checkProductTypeMandant(salesArrangement.CaseId, cancellationToken);

        return CustomerMapper.MapCustomerToResponseDto<CustomerGetCustomerDetailWithChangesResponse>(customerInfo.CustomerWithChangedData, customerInfo.CustomerOnSA);
    }

    private async Task checkProductTypeMandant(long caseId, CancellationToken cancellationToken)
    {
        var caseData = await _caseService.GetCaseDetail(caseId, cancellationToken);

        // mandant produktu
        var productMandant = (await _codebookService.ProductTypes(cancellationToken)).FirstOrDefault(t => t.Id == caseData.Data.ProductTypeId)?.MandantId
                             ?? throw new NobyValidationException($"ProductTypeId {caseData.Data.ProductTypeId} not found");

        if (productMandant != 2) // muze byt jen KB
            throw new NobyValidationException("Product type mandant is not KB");
    }

    private readonly CustomerWithChangedDataService _changedDataService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;

    public GetCustomerDetailWithChangesHandler(
        CustomerWithChangedDataService changedDataService,
        ICodebookServiceClient codebookService,
        ISalesArrangementServiceClient salesArrangementService,
        ICaseServiceClient caseService)
    {
        _changedDataService = changedDataService;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
    }
}
