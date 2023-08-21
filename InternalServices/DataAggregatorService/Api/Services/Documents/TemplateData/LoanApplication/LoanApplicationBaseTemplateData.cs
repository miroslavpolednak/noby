﻿using CIS.Core.Exceptions;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.LoanApplication;

internal abstract class LoanApplicationBaseTemplateData : AggregatedData
{
    private readonly CustomerWithChangesService _customerWithChangesService;

    protected abstract HouseholdInfo CurrentHousehold { get; }

    protected LoanApplicationBaseTemplateData(CustomerWithChangesService customerWithChangesService)
    {
        _customerWithChangesService = customerWithChangesService;
    }

    public LoanApplicationCustomer Customer1 { get; private set; } = null!;

    public LoanApplicationCustomer? Customer2 { get; private set; }

    public LoanApplicationIncome Customer1Income { get; private set; } = null!;

    public LoanApplicationIncome? Customer2Income { get; private set; }

    public LoanApplicationObligation Customer1Obligation { get; private set; } = null!;

    public LoanApplicationObligation? Customer2Obligation { get; private set; }

    public string? Customer1MaritalStatus => CurrentHousehold.Household!.Data.AreBothPartnersDeptors == true ? $"{GetMaritalStatus(Customer1)} | druh/družka" : GetMaritalStatus(Customer1);

    public string? Customer2MaritalStatus => CurrentHousehold.Household!.Data.AreBothPartnersDeptors == true ? $"{GetMaritalStatus(Customer2)} | druh/družka" : GetMaritalStatus(Customer2);

    public string PropertySettlement => GetPropertySettlementName();
    
    public string BrokerEmailAndPhone => string.Join(" | ", new[] { User.Phone, User.Email }.Where(str => !string.IsNullOrWhiteSpace(str)));

    public override Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        var identity1 = CurrentHousehold.CustomerOnSa1?.CustomerIdentifiers?.FirstOrDefault(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb) ??
                             throw new CisValidationException($"CustomerOnSa 1 {CurrentHousehold.Debtor.CustomerOnSAId} does not have KB identifier.");

        var identity2 = CurrentHousehold.CustomerOnSa2?.CustomerIdentifiers?.FirstOrDefault(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb);

        return SetDebtorAndCodebtorData(identity1, identity2, cancellationToken);
    }

    private async Task SetDebtorAndCodebtorData(Identity identity1, Identity? identity2, CancellationToken cancellationToken)
    {
        var customers = await _customerWithChangesService.GetCustomerList(new[] { CurrentHousehold.CustomerOnSa1, CurrentHousehold.CustomerOnSa2 }.OfType<CustomerOnSA>(), cancellationToken);

        Customer1 = CreateCustomer(GetDetail(identity1.IdentityId));
        Customer1Income = new LoanApplicationIncome(CurrentHousehold.CustomerOnSa1!);
        Customer1Obligation = new LoanApplicationObligation(CurrentHousehold.CustomerOnSa1!);

        if (identity2 is not null)
        {
            Customer2 = CreateCustomer(GetDetail(identity2.IdentityId));
            Customer2Income = new LoanApplicationIncome(CurrentHousehold.CustomerOnSa2!);
            Customer2Obligation = new LoanApplicationObligation(CurrentHousehold.CustomerOnSa2!);
        }

        LoanApplicationCustomer CreateCustomer(CustomerDetailResponse customer) => new(customer, _codebookManager.DegreesBefore, _codebookManager.Countries, _codebookManager.IdentificationDocumentTypes, _codebookManager.EducationLevels);
        CustomerDetailResponse GetDetail(long id) => customers.First(c => c.Identities.Any(i => i.IdentityId == id && i.IdentityScheme == Identity.Types.IdentitySchemes.Kb));
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.Countries().DegreesBefore().LoanKinds().LoanPurposes().ProductTypes()
                    .PropertySettlements().IdentificationDocumentTypes().MaritalStatuses().EducationLevels();
    }

    protected string GetLoanPurposes(int loanKindId, IEnumerable<int> loanPurposeIds)
    {
        if (loanKindId == 2001)
            return "koupě/výstavba/rekonstrukce";

        return string.Join("; ",
                           loanPurposeIds.Select(loanPurposeId => _codebookManager.LoanPurposes.Where(p => p.MandantId == 2 && p.Id == loanPurposeId)
                                                                                  .Select(p => p.Name)
                                                                                  .FirstOrDefault()));
    }

    protected string GetProductTypeName(int productTypeId) =>
        _codebookManager.ProductTypes.Where(x => x.MandantId == 2 && x.Id == productTypeId)
                        .Select(x => x.Name)
                        .DefaultIfEmpty(string.Empty)
                        .First();

    protected string GetLoanKindName(int loanKindId) =>
        _codebookManager.LoanKinds.Where(x => x.MandantId == 2 && x.Id == loanKindId)
                        .Select(x => x.Name)
                        .DefaultIfEmpty(string.Empty)
                        .First();

    private string? GetMaritalStatus(LoanApplicationCustomer? customer)
    {
        if (customer is null)
            return default;

        return _codebookManager.MaritalStatuses.Where(m => m.Id == customer.MaritalStatusStateId)
                               .Select(m => m.Name)
                               .First();
    }

    private string GetPropertySettlementName() =>
        _codebookManager.PropertySettlements.Where(p => p.Id != 0 && p.Id == CurrentHousehold.Household!.Data.PropertySettlementId)
                        .Select(p => p.Name)
                        .DefaultIfEmpty("--")
                        .First();
}