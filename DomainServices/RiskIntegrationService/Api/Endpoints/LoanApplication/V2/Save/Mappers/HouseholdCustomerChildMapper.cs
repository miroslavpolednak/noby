using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _RAT = DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;
using _CB = DomainServices.CodebookService.Contracts.Endpoints;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save.Mappers;

internal sealed class HouseholdCustomerChildMapper
{
    public async Task<List<_C4M.LoanApplicationCounterParty>> MapCustomers(List<_V2.LoanApplicationCustomer> customers)
    {
        var countries = await _codebookService.Countries(_cancellationToken);
        var customerRoles = await _codebookService.CustomerRoles(_cancellationToken);
        var genders = await _codebookService.Genders(_cancellationToken);
        var maritalStatuses = await _codebookService.MaritalStatuses(_cancellationToken);
        var educations = await _codebookService.EducationLevels(_cancellationToken);
        var housingConditions = await _codebookService.HousingConditions(_cancellationToken);
        var identificationDocuments = await _codebookService.IdentificationDocumentTypes(_cancellationToken);

        return null;
    }
        

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly CancellationToken _cancellationToken;
    private readonly _RAT.RiskApplicationTypeItem _riskApplicationType;

    public HouseholdCustomerChildMapper(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        _RAT.RiskApplicationTypeItem riskApplicationType,
        CancellationToken cancellationToken)
    {
        _riskApplicationType = riskApplicationType;
        _cancellationToken = cancellationToken;
        _codebookService = codebookService;
    }
}
