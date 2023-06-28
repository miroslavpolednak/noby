using DomainServices.CaseService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.CodebookService.Clients;

namespace NOBY.Api.Endpoints.Customer.GetCustomerDetailWithChanges;

internal sealed class GetCustomerDetailWithChangesHandler
    : IRequestHandler<GetCustomerDetailWithChangesRequest, GetCustomerDetailWithChangesResponse>
{
    public async Task<GetCustomerDetailWithChangesResponse> Handle(GetCustomerDetailWithChangesRequest request, CancellationToken cancellationToken)
    {
        // customer instance
        var customerOnSA = await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken);

        // SA instance
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(customerOnSA.SalesArrangementId, cancellationToken);

        await CheckProductTypeMandant(salesArrangement.CaseId, cancellationToken);

        var (data, _) = await _changedDataService.GetCustomerWithChangedData<GetCustomerDetailWithChangesResponse>(customerOnSA, cancellationToken);
        return data;
    }

    private async Task CheckProductTypeMandant(long caseId, CancellationToken cancellationToken)
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
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICaseServiceClient _caseService;

    public GetCustomerDetailWithChangesHandler(
        CustomerWithChangedDataService changedDataService,
        ICodebookServiceClient codebookService,
        ISalesArrangementServiceClient salesArrangementService,
        ICustomerOnSAServiceClient customerOnSAService,
        ICaseServiceClient caseService)
    {
        _changedDataService = changedDataService;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _customerOnSAService = customerOnSAService;
        _caseService = caseService;
    }
}
