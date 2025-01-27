﻿using CIS.Infrastructure.Caching.Grpc;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;
using FastEnumUtility;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.v1.DeleteHousehold;

//TODO tady by asi mel byt nejaky rollback, kdyz se nepodari smazat customer? Co se ma mazat driv - customer nebo household?
internal sealed class DeleteHouseholdHandler(
    IGrpcServerResponseCache _responseCache,
    HouseholdServiceDbContext _dbContext,
    IMediator _mediator,
    SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService,
    IDocumentOnSAServiceClient _documentOnSAServiceClient)
        : IRequestHandler<DeleteHouseholdRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteHouseholdRequest request, CancellationToken cancellationToken)
    {
        var household = await _dbContext
            .Households
            .AsNoTracking()
            .Where(t => t.HouseholdId == request.HouseholdId)
            .Select(t => new { t.SalesArrangementId, t.CustomerOnSAId1, t.CustomerOnSAId2, t.HouseholdTypeId })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.HouseholdNotFound, request.HouseholdId);

        // kontrola ze to neni main household
        if (household.HouseholdTypeId == HouseholdTypes.Main && !request.HardDelete)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.CantDeleteDebtorHousehold);
        }

        //Invalidate DocumentOnSa (stop signing)
        var documentsOnSaToSing = await _documentOnSAServiceClient.GetDocumentsToSignList(household.SalesArrangementId, cancellationToken);
        // With household
        var documentsOnSaWithHousehold = documentsOnSaToSing.DocumentsOnSAToSign.Where(d => d.HouseholdId == request.HouseholdId && d.DocumentOnSAId is not null);
        await StopSigning(documentsOnSaWithHousehold, cancellationToken);
        // Crs
        var documentsOnSaCrs = documentsOnSaToSing.DocumentsOnSAToSign.Where(r => r.DocumentOnSAId is not null && r.DocumentTypeId == DocumentTypes.DANRESID.ToByte() &&
                                                                            (r.CustomerOnSA?.CustomerOnSAId == household.CustomerOnSAId1 || r.CustomerOnSA?.CustomerOnSAId == household.CustomerOnSAId2));
        await StopSigning(documentsOnSaCrs, cancellationToken);

        // smazat domacnost
        await _dbContext
            .Households
            .Where(t => t.HouseholdId == request.HouseholdId)
            .ExecuteDeleteAsync(cancellationToken);

        // smazat customerOnSA
        if (household.CustomerOnSAId1.HasValue)
        {
            await _mediator.Send(new DeleteCustomerRequest
            {
                CustomerOnSAId = household.CustomerOnSAId1.Value
            }, cancellationToken);
        }

        if (household.CustomerOnSAId2.HasValue)
        {
            await _mediator.Send(new DeleteCustomerRequest
            {
                CustomerOnSAId = household.CustomerOnSAId2.Value
            }, cancellationToken);
        }

        // pokud se jedna o spoludluznickou, smazat flowswitch
        if (household.HouseholdTypeId == HouseholdTypes.Codebtor)
        {
            await _salesArrangementService.SetFlowSwitches(household.SalesArrangementId, new List<SalesArrangementService.Contracts.EditableFlowSwitch>
            {
                new()
                {
                    FlowSwitchId = (int)FlowSwitches.CustomerIdentifiedOnCodebtorHousehold,
                    Value = null
                },
                new()
                {
                    FlowSwitchId = (int)FlowSwitches.Was3602CodebtorChangedAfterSigning,
                    Value = null
                }
            }, cancellationToken);
        }

        await _responseCache.InvalidateEntry(nameof(GetHouseholdList), household.SalesArrangementId);

        return new Google.Protobuf.WellKnownTypes.Empty();
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
}