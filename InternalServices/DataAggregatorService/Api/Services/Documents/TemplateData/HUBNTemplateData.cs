﻿using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using System.Globalization;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

internal class HUBNTemplateData : AggregatedData
{
    public string PaymentAccount => BankAccountHelper.AccountNumber(Mortgage.PaymentAccount.Prefix, Mortgage.PaymentAccount.Number, Mortgage.PaymentAccount.BankCode);

    public string FullName => CustomerHelper.FullName(Customer, _codebookManager.DegreesBefore);

    public string PermanentAddress => CustomerHelper.FullAddress(Customer, AddressTypes.Permanent);

    public IEnumerable<string> LoanPurposes => GetLoanPurposes();

    public IEnumerable<string> LoanRealEstates => GetLoanRealEstates();

    public string DrawingDateToText
    {
        get
        {
            if (SalesArrangement.HUBN.DrawingDateTo?.IsActive != true || SalesArrangement.HUBN.DrawingDateTo.ExtensionDrawingDateToByMonths <= 0)
                return "--";

            var monthText = SalesArrangement.HUBN.DrawingDateTo.ExtensionDrawingDateToByMonths!.Value switch
            {
                1 => "měsíc",
                2 or 3 or 4 => "měsíce",
                _ => "měsíců"
            };

            var agreedDrawingToText = ((DateTime)SalesArrangement.HUBN.DrawingDateTo.AgreedDrawingDateTo).ToString("d", CultureProvider.GetProvider());

            return $"o {SalesArrangement.HUBN.DrawingDateTo.ExtensionDrawingDateToByMonths} {monthText} od původní lhůty čerpání {agreedDrawingToText}";
        }
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DegreesBefore().LoanPurposes().RealEstateTypes().PurchaseTypes();
    }

    private IEnumerable<string> GetLoanPurposes()
    {
        var numberFormat = (NumberFormatInfo)CultureProvider.GetProvider().GetFormat(typeof(NumberFormatInfo))!;

        var loanPurposes = SalesArrangement.HUBN
                                           .LoanPurposes
                                           .Join(_codebookManager.LoanPurposes.Where(l => l.MandantId == 2), x => x.LoanPurposeId, y => y.Id, (x, y) => new { y.Name, x.Sum })
                                           .Select(p => string.Format(numberFormat, $"{p.Name}: " + "{0:#,0.##}" + $" {numberFormat.CurrencySymbol}", (decimal)p.Sum))
                                           .ToList();

        if (loanPurposes.Count < 5)
            loanPurposes.Add("--");

        return loanPurposes;
    }

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

        var result = realEstates.Select(r => r.LoanRealEstate.IsCollateral
                                            ? $"{r.RealEstateTypeName}, {r.PurchaseTypeName}, slouží k zajištění"
                                            : $"{r.RealEstateTypeName}, {r.PurchaseTypeName}")
                                .ToList();

        if (result.Count < 3)
            result.Add("--");

        return result;
    }
}