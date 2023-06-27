using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.LoanApplication;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

[TransientService, SelfService]
internal class CustomerChange3602TemplateData : LoanApplicationBaseTemplateData
{
    protected override HouseholdInfo CurrentHousehold => HouseholdCodebtor!;

    public CustomerChange3602TemplateData(CustomerWithChangesService customerWithChangesService) : base(customerWithChangesService)
    {
    }

    public string LoanDurationText => "Předpokládané datum splatnosti";
    
    public string LoanType => Mortgage.LoanKindId == 2001 ? GetLoanKindName(Mortgage.LoanKindId ?? 0) : GetProductTypeName(Mortgage.ProductTypeId);

    public string LoanPurposes => GetLoanPurposes(Mortgage.LoanKindId ?? 0, Mortgage.LoanPurposes.Select(l => l.LoanPurposeId));
}