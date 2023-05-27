using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.LoanApplication;
using DomainServices.CustomerService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

[TransientService, SelfService]
internal class LoanApplication3602TemplateData : LoanApplicationBaseTemplateData
{
    protected override HouseholdInfo CurrentHousehold => HouseholdCodebtor!;

    public LoanApplication3602TemplateData(ICustomerServiceClient customerService) : base(customerService)
    {
    }

    public string LoanType => Offer.SimulationInputs.LoanKindId == 2001 ? GetLoanKindName(Offer.SimulationInputs.LoanKindId) : GetProductTypeName(Offer.SimulationInputs.ProductTypeId);

    public string LoanPurposes => GetLoanPurposes(Offer.SimulationInputs.LoanKindId, Offer.SimulationInputs.LoanPurposes.Select(l => l.LoanPurposeId));
}