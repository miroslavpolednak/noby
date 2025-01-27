﻿using CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.Shared;
using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData;

internal class GeneralChangeTemplateData : AggregatedData
{
    protected SalesArrangementParametersGeneralChange GeneralChange => SalesArrangement.GeneralChange;

    public string PaymentAccount => BankAccountHelper.AccountNumber(Mortgage.PaymentAccount.Prefix, Mortgage.PaymentAccount.Number, Mortgage.PaymentAccount.BankCode);

    public string FullName => CustomerHelper.FullName(Customer.Source, _codebookManager.DegreesBefore);

    public string SignerName => CustomerHelper.FullName(Customer.Source);

    public string PermanentAddress => CustomerHelper.FullAddress(Customer.Source, AddressTypes.Permanent);

    public string? RepaymentAccount
    {
        get
        {
            if (GeneralChange.RepaymentAccount?.IsActive != true)
                return default;

            return BankAccountHelper.AccountNumber(GeneralChange.RepaymentAccount.Prefix, GeneralChange.RepaymentAccount.Number, GeneralChange.RepaymentAccount.BankCode);
        }
    }

    public string? RepaymentAccountOwner
    {
        get
        {
            if (GeneralChange.RepaymentAccount?.IsActive != true)
                return default;

            return CustomerHelper.NameWithDateOfBirth($"{GeneralChange.RepaymentAccount.OwnerFirstName} {GeneralChange.RepaymentAccount.OwnerLastName}", GeneralChange.RepaymentAccount.OwnerDateOfBirth);
        }
    }

    public string? RealEstateTypes => GeneralChange.LoanRealEstate?.IsActive == true ? string.Join("; ", GetRealEstateTypes()) : default;

    public string? RealEstatePurchaseTypes => GeneralChange.LoanRealEstate?.IsActive == true ? string.Join("; ", GetRealEstatePurchaseTypes()) : default;

    public string? ExtensionDrawingDateText
    {
        get
        {
            if (GeneralChange.DrawingDateTo?.IsActive != true || (GeneralChange.DrawingDateTo.ExtensionDrawingDateToByMonths ?? 0) <= 0)
                return default;

            var months = Math.Abs(GeneralChange.DrawingDateTo.ExtensionDrawingDateToByMonths!.Value);

            var monthsText = months switch
            {
                1 => "měsíc",
                2 or 3 or 4 => "měsíce",
                _ => "měsíců"
            };

            return $"o {months} {monthsText} od původní lhůty čerpání  {((DateTime)GeneralChange.DrawingDateTo.AgreedDrawingDateTo).ToString("d", CultureProvider.GetProvider())}";
        }
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DegreesBefore().RealEstateTypes().PurchaseTypes();
    }

    private IEnumerable<string> GetRealEstateTypes() =>
        GeneralChange.LoanRealEstate
                     .LoanRealEstates
                     .Join(_codebookManager.RealEstateTypes, x => x.RealEstateTypeId, y => y.Id, (_, y) => y.Name);

    private IEnumerable<string> GetRealEstatePurchaseTypes() =>
        GeneralChange.LoanRealEstate
                     .LoanRealEstates
                     .Join(_codebookManager.PurchaseTypes, x => x.RealEstatePurchaseTypeId, y => y.Id, (_, y) => y.Name.Trim());
}