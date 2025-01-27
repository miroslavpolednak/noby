﻿using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.DeleteRealEstateValuationAttachment;

internal sealed class DeleteRealEstateValuationAttachmentHandler(
    RealEstateValuationServiceDbContext _dbContext,
    ExternalServices.PreorderService.V1.IPreorderServiceClient _preorderService,
    ILogger<DeleteRealEstateValuationAttachmentHandler> _logger)
        : IRequestHandler<DeleteRealEstateValuationAttachmentRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteRealEstateValuationAttachmentRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .Attachments
            .FirstOrDefaultAsync(t => t.RealEstateValuationAttachmentId == request.RealEstateValuationAttachmentId, cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationAttachmentNotFound, request.RealEstateValuationAttachmentId);

        if (entity.RealEstateValuationId != request.RealEstateValuationId)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationAttachmentNotFound, request.RealEstateValuationAttachmentId);
        }

        _dbContext.Attachments.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // odstranit z ACV - je nam jedno jestli se to povede, takze do try catch
        try
        {
            await _preorderService.DeleteAttachment(entity.ExternalId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.AttachmentDeleteFailed(entity.ExternalId, request.RealEstateValuationAttachmentId, ex);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
