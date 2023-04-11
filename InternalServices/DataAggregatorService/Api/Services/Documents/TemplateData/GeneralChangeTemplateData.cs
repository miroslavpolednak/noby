using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

internal class GeneralChangeTemplateData : AggregatedData
{
    protected SalesArrangementParametersGeneralChange GeneralChange => SalesArrangement.GeneralChange;

    public string PaymentAccount => BankAccountHelper.AccountNumber(Mortgage.PaymentAccount.Prefix, Mortgage.PaymentAccount.Number, Mortgage.PaymentAccount.BankCode);

    public string FullName => CustomerHelper.FullName(Customer, _codebookManager.DegreesBefore);

    public string PermanentAddress => CustomerHelper.FullAddress(Customer, _codebookManager.Countries);

    public string RepaymentAccount => 
        BankAccountHelper.AccountNumber(GeneralChange.RepaymentAccount.Prefix, GeneralChange.RepaymentAccount.Number, GeneralChange.RepaymentAccount.BankCode);

    public string RepaymentAccountOwner =>
        CustomerHelper.NameWithDateOfBirth($"{GeneralChange.RepaymentAccount.OwnerFirstName} {GeneralChange.RepaymentAccount.OwnerLastName}", GeneralChange.RepaymentAccount.OwnerDateOfBirth);

    public string RealEstateTypes => string.Join("; ", GetRealEstateTypes());

    public string RealEstatePurchaseTypes => string.Join("; ", GetRealEstatePurchaseTypes());

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.Countries().DegreesBefore().RealEstateTypes().PurchaseTypes();
    }

    private IEnumerable<string> GetRealEstateTypes() =>
        GeneralChange.LoanRealEstate
                     .LoanRealEstates
                     .Join(_codebookManager.RealEstateTypes, x => x.RealEstateTypeId, y => y.Id, (_, y) => y.Name);

    private IEnumerable<string> GetRealEstatePurchaseTypes() =>
        GeneralChange.LoanRealEstate
                     .LoanRealEstates
                     .Join(_codebookManager.PurchaseTypes, x => x.RealEstatePurchaseTypeId, y => y.Id, (_, y) => y.Name);
}