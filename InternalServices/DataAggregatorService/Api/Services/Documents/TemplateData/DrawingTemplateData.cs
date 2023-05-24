using System.Globalization;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData;

internal class DrawingTemplateData : AggregatedData
{
    public string PersonName => CustomerHelper.FullName(Customer, _codebookManager.DegreesBefore);

    public string PersonAddress => CustomerHelper.FullAddress(Customer, _codebookManager.Countries);

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
            if (SalesArrangement.Drawing.RepaymentAccount is null)
                return string.Empty;

            var account = SalesArrangement.Drawing.RepaymentAccount;

            return "Číslo účtu pro splácení úvěru: " + $"{BankAccountHelper.AccountNumber(account.Prefix, account.Number, account.BankCode)}";
        }
    }

    public string SignPersonName => SalesArrangement.Drawing.Agent?.IsActive == true ? string.Empty : CustomerHelper.FullName(Customer);

    public string SignAgentName => SalesArrangement.Drawing.Agent?.IsActive != true ? string.Empty : $"{SalesArrangement.Drawing.Agent.FirstName} {SalesArrangement.Drawing.Agent.LastName}";

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.Countries().DegreesBefore();
    }
}