﻿using CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.Shared;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData;

internal class DrawingTemplateData : AggregatedData
{
    public string PersonName => CustomerHelper.FullName(Customer.Source, _codebookManager.DegreesBefore);

    public string PersonAddress => CustomerHelper.FullAddress(Customer.Source, AddressTypes.Permanent);

    public string PaymentAccount
    {
        get
        {
            var bankAccount = BankAccountHelper.AccountNumber(Mortgage.PaymentAccount.Prefix, Mortgage.PaymentAccount.Number, Mortgage.PaymentAccount.BankCode);

            if (SalesArrangement.Drawing.IsImmediateDrawing)
                return bankAccount + " a to bezodkladně.";

            return bankAccount + $" a to k datu: {((DateTime)SalesArrangement.Drawing.DrawingDate).ToString("d", CultureInfo.GetCultureInfo("cs"))}.";
        }
    }

    public string RepaymentAccount
    {
        get
        {
            if (SalesArrangement.Drawing.RepaymentAccount?.IsAccountNumberMissing != true)
                return string.Empty;

            var account = SalesArrangement.Drawing.RepaymentAccount;

            return "Číslo účtu pro splácení úvěru: " + $"{BankAccountHelper.AccountNumber(account.Prefix, account.Number, account.BankCode)}";
        }
    }

    public string SignPersonName => SalesArrangement.Drawing.Agent?.IsActive == true ? string.Empty : CustomerHelper.FullName(Customer.Source);

    public string SignAgentName => SalesArrangement.Drawing.Agent?.IsActive != true ? string.Empty : $"{SalesArrangement.Drawing.Agent.FirstName} {SalesArrangement.Drawing.Agent.LastName}";

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DegreesBefore();
    }
}