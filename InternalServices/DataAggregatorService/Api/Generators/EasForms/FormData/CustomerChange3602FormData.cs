﻿using CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData.LoanApplicationData;
using CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData.ProductRequest;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData;

[TransientService, SelfService]
internal class CustomerChange3602FormData : LoanApplicationBaseFormData
{
    public CustomerChange3602FormData(HouseholdData householdData) : base(householdData)
    {
    }

    public bool ChangeProposal => (SalesArrangementTypes)SalesArrangement.SalesArrangementTypeId is SalesArrangementTypes.CustomerChange3602A or SalesArrangementTypes.CustomerChange3602C;

    public int ContractSignatureTypeId => 1;

    public override Task LoadAdditionalData(InputParameters parameters, CancellationToken cancellationToken)
    {
        HouseholdData.IsSpouseInDebt = GetIsSpouseIsDebt();

        return base.LoadAdditionalData(parameters,cancellationToken);
    }

    private bool? GetIsSpouseIsDebt()
    {
        if (HouseholdData.HouseholdDto is { CustomerOnSaId1: not null, CustomerOnSaId2: not null })
            return false;

        return ((SalesArrangementTypes)SalesArrangement.SalesArrangementTypeId switch
        {
            SalesArrangementTypes.CustomerChange3602A => SalesArrangement.CustomerChange3602A.IsSpouseInDebt,
            SalesArrangementTypes.CustomerChange3602B => SalesArrangement.CustomerChange3602B.IsSpouseInDebt,
            SalesArrangementTypes.CustomerChange3602C => SalesArrangement.CustomerChange3602C.IsSpouseInDebt,
            _ => throw new NotImplementedException()
        }) ?? false;
    }
}