using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts;
using Codebook = DomainServices.CodebookService.Contracts.Endpoints;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.LoanApplication;

internal class LoanApplicationBaseTemplateData : AggregatedData
{
    private List<Codebook.ProductTypes.ProductTypeItem> _productTypes = null!;
    private List<Codebook.LoanKinds.LoanKindsItem> _loanKinds = null!;
    private List<Codebook.LoanPurposes.LoanPurposesItem> _loanPurposes = null!;
    private List<Codebook.PropertySettlements.PropertySettlementItem> _propertySettlements = null!;
    protected List<GenericCodebookItem> _degreesBefore = null!;
    protected List<Codebook.Countries.CountriesItem> _countries = null!;
    protected List<Codebook.IdentificationDocumentTypes.IdentificationDocumentTypesItem> _identificationDocumentTypes = null!;
    protected List<Codebook.MaritalStatuses.MaritalStatusItem> _maritalStatuses = null!;

    public string LoanApplicationHeader1 => IsUserBroker() ? string.Empty : "ŽÁDOST O POSKYTNUTÍ ÚVĚRU";

    public string LoanApplicationHeader2 => IsUserBroker() ? "ŽÁDOST O POSKYTNUTÍ ÚVĚRU A POTVRZENÍ O ZPROSTŘEDKOVÁNÍ" : string.Empty;

    public string LoanType => Offer.SimulationInputs.LoanKindId == 2001 ? GetLoanKindName() : GetProductTypeName();

    public string LoanPurposes
    {
        get
        {
            if (Offer.SimulationInputs.LoanKindId == 2001)
                return "koupě/výstavba/rekonstrukce";

            return string.Join("; ",
                               Offer.SimulationInputs
                                    .LoanPurposes
                                    .Select(x => _loanPurposes.Where(p => p.MandantId == 2 && p.Id == x.LoanPurposeId)
                                                              .Select(p => p.Name)
                                                              .FirstOrDefault()));
        }
    }

    public string PropertySettlement => GetPropertySettlementName();

    public override async Task LoadCodebooks(ICodebookServiceClients codebookService)
    {
        _productTypes = await codebookService.ProductTypes();
        _loanKinds = await codebookService.LoanKinds();
        _loanPurposes = await codebookService.LoanPurposes();
        _propertySettlements = await codebookService.PropertySettlements();
        _degreesBefore = await codebookService.AcademicDegreesBefore();
        _countries = await codebookService.Countries();
        _identificationDocumentTypes = await codebookService.IdentificationDocumentTypes();
        _maritalStatuses = await codebookService.MaritalStatuses();
    }

    private bool IsUserBroker() => User.UserIdentifiers.Any(u => u.IdentityScheme == UserIdentity.Types.UserIdentitySchemes.BrokerId);

    private string GetProductTypeName() =>
        _productTypes.Where(x => x.MandantId == 2 && x.Id == Offer.SimulationInputs.ProductTypeId)
                     .Select(x => x.Name.ToUpperInvariant())
                     .DefaultIfEmpty(string.Empty)
                     .First();

    private string GetLoanKindName() =>
        _loanKinds.Where(x => x.MandantId == 2 && x.Id == Offer.SimulationInputs.LoanKindId)
                  .Select(x => x.Name.ToUpperInvariant())
                  .DefaultIfEmpty(string.Empty)
                  .First();

    private string GetPropertySettlementName() =>
        _propertySettlements.Where(p => p.Id == HouseholdMain.Household.Data.PropertySettlementId)
                            .Select(p => p.Name)
                            .DefaultIfEmpty(string.Empty)
                            .First();
}