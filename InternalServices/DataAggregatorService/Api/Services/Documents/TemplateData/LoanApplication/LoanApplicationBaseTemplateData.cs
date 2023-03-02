using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.LoanApplication;

internal class LoanApplicationBaseTemplateData : AggregatedData
{
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
                                    .Select(x => _codebookManager.LoanPurposes.Where(p => p.MandantId == 2 && p.Id == x.LoanPurposeId)
                                                                 .Select(p => p.Name)
                                                                 .FirstOrDefault()));
        }
    }

    public string PropertySettlement => GetPropertySettlementName();

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.Countries().DegreesBefore().LoanKinds().LoanPurposes().ProductTypes()
                    .PropertySettlements().IdentificationDocumentTypes().MaritalStatuses();
    }

    private bool IsUserBroker() => User.UserIdentifiers.Any(u => u.IdentityScheme == UserIdentity.Types.UserIdentitySchemes.BrokerId);

    private string GetProductTypeName() =>
        _codebookManager.ProductTypes.Where(x => x.MandantId == 2 && x.Id == Offer.SimulationInputs.ProductTypeId)
                        .Select(x => x.Name)
                        .DefaultIfEmpty(string.Empty)
                        .First();

    private string GetLoanKindName() =>
        _codebookManager.LoanKinds.Where(x => x.MandantId == 2 && x.Id == Offer.SimulationInputs.LoanKindId)
                        .Select(x => x.Name)
                        .DefaultIfEmpty(string.Empty)
                        .First();

    private string GetPropertySettlementName() =>
        _codebookManager.PropertySettlements.Where(p => p.Id == HouseholdMain.Household.Data.PropertySettlementId)
                        .Select(p => p.Name)
                        .DefaultIfEmpty(string.Empty)
                        .First();
}