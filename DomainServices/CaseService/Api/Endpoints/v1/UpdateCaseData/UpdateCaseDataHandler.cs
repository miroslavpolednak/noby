﻿using CIS.Infrastructure.Caching.Grpc;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.v1.UpdateCaseData;

internal sealed class UpdateCaseDataHandler(
    ILogger<UpdateCaseDataHandler> _logger,
    IGrpcServerResponseCache _responseCache,
    IMediator _mediator,
    CodebookService.Clients.ICodebookServiceClient _codebookService,
    CaseServiceDbContext _dbContext)
        : IRequestHandler<UpdateCaseDataRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateCaseDataRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        var entity = await _dbContext
            .Cases
            .FirstOrDefaultAsync(t => t.CaseId == request.CaseId, cancellation)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        // zkontrolovat ProdInstType
        if (!(await _codebookService.ProductTypes(cancellation)).Any(t => t.Id == request.Data.ProductTypeId))
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.ProductTypeIdNotFound, request.Data.ProductTypeId);
        }

        var bonusChanged = entity.IsEmployeeBonusRequested != request.Data.IsEmployeeBonusRequested;

        // ulozit do DB
        entity.ContractNumber = request.Data.ContractNumber;
        entity.TargetAmount = request.Data.TargetAmount;
        entity.IsEmployeeBonusRequested = request.Data.IsEmployeeBonusRequested;
        entity.ProductTypeId = request.Data.ProductTypeId;

        await _dbContext.SaveChangesAsync(cancellation);

        await _responseCache.InvalidateEntry(nameof(GetCaseDetail), request.CaseId);

        // pokud se zmenil IsEmployeeBonusRequested, zavolat EAS
        if (bonusChanged)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                await _mediator.Send(new NotifyStarbuildRequest
                {
                    CaseId = request.CaseId
                }, cancellation);
            }
            catch (Exception ex)
            {
                // pouze logujeme!
                _logger.CaseStateChangedFailed(request.CaseId, ex);
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
