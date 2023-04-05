using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

internal class HUBNTemplateData : AggregatedData
{
    public string PaymentAccount => BankAccountHelper.AccountNumber(Mortgage.PaymentAccount.Prefix, Mortgage.PaymentAccount.Number, Mortgage.PaymentAccount.BankCode);

    public string FullName => CustomerHelper.FullName(Customer, _codebookManager.DegreesBefore);

    public string PermanentAddress => CustomerHelper.FullAddress(Customer, _codebookManager.Countries);

    public IEnumerable<string> LoanPurposes => GetLoanPurposes();

    public IEnumerable<string> LoanRealEstates => GetLoanRealEstates();

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.Countries().DegreesBefore().LoanPurposes().RealEstateTypes().PurchaseTypes();
    }

    private IEnumerable<string> GetLoanPurposes() =>
        SalesArrangement.HUBN
                        .LoanPurposes
                        .Join(_codebookManager.LoanPurposes.Where(l => l.MandantId == 2), x => x.LoanPurposeId, y => y.Id, (_, y) => y.Name);

    private IEnumerable<string> GetLoanRealEstates()
    {
        var realEstates = from l in SalesArrangement.HUBN.LoanRealEstates
            join r in _codebookManager.RealEstateTypes on l.RealEstateTypeId equals r.Id
            join p in _codebookManager.PurchaseTypes on l.RealEstatePurchaseTypeId equals p.Id
            select new
            {
                LoanRealEstate = l,
                RealEstateTypeName = r.Name.Trim(),
                PurchaseTypeName = p.Name.Trim()
            };

        return realEstates.Select(r => r.LoanRealEstate.IsCollateral
                                      ? $"{r.RealEstateTypeName}, {r.PurchaseTypeName}, slouží k zajištění"
                                      : $"{r.RealEstateTypeName}, {r.PurchaseTypeName}");
    }
}