﻿using CIS.Infrastructure.Caching.Grpc;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.DeleteCustomer;

internal sealed class DeleteCustomerHandler(
    IGrpcServerResponseCache _responseCache,
    IDocumentDataStorage _documentDataStorage,
    SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService,
    Database.HouseholdServiceDbContext _dbContext,
    IDocumentOnSAServiceClient _documentOnSAServiceClient)
        : IRequestHandler<DeleteCustomerRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        // instance customera z DB
        var customer = await _dbContext
            .Customers
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .Select(t => new { t.CustomerRoleId, t.SalesArrangementId, t.Identities })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CustomerOnSANotFound, request.CustomerOnSAId);

        // KB identita pokud existuje
        var kbIdentity = customer
            .Identities?
            .FirstOrDefault(t => t.IdentityScheme == IdentitySchemes.Kb);

        // Invalidate DocumentsOnSa Crs
        var documentsOnSaToSing = await _documentOnSAServiceClient.GetDocumentsToSignList(customer.SalesArrangementId, cancellationToken);
        var documentsOnSaCrs = documentsOnSaToSing.DocumentsOnSAToSign.Where(r => r.DocumentOnSAId is not null && r.CustomerOnSA?.CustomerOnSAId == request.CustomerOnSAId);
        await StopSigning(documentsOnSaCrs, cancellationToken);

        // smazat customer + prijmy + obligations + identities
        await deleteEntities(request.CustomerOnSAId, cancellationToken);

        // smazat Agent z SA, pokud je Agent=aktualni CustomerOnSAId
        var saInstance = await _salesArrangementService.GetSalesArrangement(customer.SalesArrangementId, cancellationToken);
        if (saInstance.Mortgage?.Agent == request.CustomerOnSAId)
        {
            await deleteAgentFromSalesArrangement(saInstance, customer.SalesArrangementId, cancellationToken);
        }

        await _responseCache.InvalidateEntry(nameof(GetCustomerList), customer.SalesArrangementId);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private async Task deleteEntities(int customerOnSAId, CancellationToken cancellationToken)
    {
        await _dbContext.Customers
            .Where(t => t.CustomerOnSAId == customerOnSAId)
            .ExecuteDeleteAsync(cancellationToken);

        // smazat identity
        await _dbContext.CustomersIdentities
            .Where(t => t.CustomerOnSAId == customerOnSAId)
            .ExecuteDeleteAsync(cancellationToken);

        // smazat data customera
        await _documentDataStorage.DeleteByEntityId<int, Database.DocumentDataEntities.CustomerOnSAData>(customerOnSAId);

        // smazat prijmy
        await _documentDataStorage.DeleteByEntityId<int, Database.DocumentDataEntities.Income>(customerOnSAId);

        // smazat zavazky
        await _documentDataStorage.DeleteByEntityId<int, Database.DocumentDataEntities.Obligation>(customerOnSAId);
    }

    private async Task StopSigning(IEnumerable<DocumentOnSAToSign> documentOnSAToSigns, CancellationToken cancellationToken)
    {
        foreach (var doc in documentOnSAToSigns)
        {
            // Have to be call one by one
            await _documentOnSAServiceClient.StopSigning(new()
            {
                DocumentOnSAId = doc.DocumentOnSAId!.Value,
                SkipValidations = true
            }, cancellationToken);
        }
    }

    private async Task deleteAgentFromSalesArrangement(_SA.SalesArrangement saInstance, int salesArrangementId, CancellationToken cancellationToken)
    {
        // ziskat ID hlavniho customera
        int? mainCustomerOnSAId = (await _dbContext
            .Households
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.SalesArrangementId == salesArrangementId && t.HouseholdTypeId == HouseholdTypes.Main, cancellationToken)
        )?.CustomerOnSAId1;

        saInstance.Mortgage.Agent = mainCustomerOnSAId;

        await _salesArrangementService.UpdateSalesArrangementParameters(new _SA.UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangementId,
            Mortgage = saInstance.Mortgage
        }, cancellationToken);
    }
}