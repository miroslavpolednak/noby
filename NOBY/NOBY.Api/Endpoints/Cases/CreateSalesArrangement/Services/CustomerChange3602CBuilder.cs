﻿using SharedTypes.Enums;
using DomainServices.HouseholdService.Contracts;
using NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class CustomerChange3602CBuilder
    : BaseBuilder
{
    public override async Task PostCreateProcessing(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        var salesArrangementService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient>();
        var customerOnSAService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient>();
        var householdService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.HouseholdService.Clients.IHouseholdServiceClient>();

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
            CustomerRoleId = (int)CustomerRoles.Codebtor,
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

    public CustomerChange3602CBuilder(ILogger<CreateSalesArrangementParametersFactory> logger, __SA.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request, httpContextAccessor)
    {
    }
}
