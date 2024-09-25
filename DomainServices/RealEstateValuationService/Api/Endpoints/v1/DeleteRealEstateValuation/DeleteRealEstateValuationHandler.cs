using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.DeleteRealEstateValuation;

internal sealed class DeleteRealEstateValuationHandler(
    IDocumentDataStorage _documentDataStorage,
    RealEstateValuationServiceDbContext _dbContext,
    ILogger<DeleteRealEstateValuationHandler> _logger,
    ExternalServices.PreorderService.V1.IPreorderServiceClient _preorderService)
        : IRequestHandler<DeleteRealEstateValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .RealEstateValuations
            .FirstOrDefaultAsync(t => t.RealEstateValuationId == request.RealEstateValuationId, cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

        // seznam priloh
        var attachments = await _dbContext.Attachments
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .Select(t => new { t.RealEstateValuationAttachmentId, t.ExternalId })
            .ToListAsync(cancellationToken);

        // smazat prilohy
        await _dbContext.Attachments
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .ExecuteDeleteAsync(cancellationToken);

        // smazat DEEDs
        await _dbContext.DeedOfOwnershipDocuments
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .ExecuteDeleteAsync(cancellationToken);

        // ulozit do DB
        _dbContext.RealEstateValuations.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // smazat data
        await _documentDataStorage.Delete<Database.DocumentDataEntities.RealEstateValudationData>(request.RealEstateValuationId);

        // odstranit z ACV - je nam jedno jestli se to povede, takze do try catch
        if (attachments.Count != 0)
        {
            foreach (var attachment in attachments)
            {
                try
                {
                    await _preorderService.DeleteAttachment(attachment.ExternalId, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.AttachmentDeleteFailed(attachment.ExternalId, attachment.RealEstateValuationAttachmentId, ex);
                }
            }
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
