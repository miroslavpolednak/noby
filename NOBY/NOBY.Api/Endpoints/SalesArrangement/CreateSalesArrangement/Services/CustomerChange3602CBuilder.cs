﻿using DomainServices.HouseholdService.Contracts;
using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services;

internal sealed class CustomerChange3602CBuilder
    : BaseBuilder
{
    public override async Task PostCreateProcessing(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        var salesArrangementService = GetRequiredService<DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient>();
        var customerOnSAService = GetRequiredService<DomainServices.HouseholdService.Clients.v1.ICustomerOnSAServiceClient>();
        var householdService = GetRequiredService<DomainServices.HouseholdService.Clients.v1.IHouseholdServiceClient>();

        // vytvorit domacnost
        var requestModel = new CreateHouseholdRequest
        {
            SalesArrangementId = salesArrangementId,
            HouseholdTypeId = (int)HouseholdTypes.Codebtor
        };

        var householdId = await householdService.CreateHousehold(requestModel, cancellationToken);

        // vytvorit klienta
        var createCustomerResult = await customerOnSAService.CreateCustomer(new CreateCustomerRequest
        {
            CustomerRoleId = (int)EnumCustomerRoles.Codebtor,
            SalesArrangementId = salesArrangementId
        }, cancellationToken);

        await householdService.LinkCustomerOnSAToHousehold(householdId, createCustomerResult.CustomerOnSAId, null, cancellationToken);

        // update parametru
        await salesArrangementService.UpdateSalesArrangementParameters(new __SA.UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangementId,
            CustomerChange3602C = new __SA.SalesArrangementParametersCustomerChange3602
            {
                HouseholdId = householdId
            }
        }, cancellationToken);
    }

    public CustomerChange3602CBuilder(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }
}
