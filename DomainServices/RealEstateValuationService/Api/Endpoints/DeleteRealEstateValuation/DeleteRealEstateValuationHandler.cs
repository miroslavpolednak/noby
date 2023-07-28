using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.DeleteRealEstateValuation;

internal sealed class DeleteRealEstateValuationHandler
    : IRequestHandler<DeleteRealEstateValuationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .RealEstateValuations
            .FirstOrDefaultAsync(t => t.RealEstateValuationId == request.RealEstateValuationId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);

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

        // odstranit z ACV - je nam jedno jestli se to povede, takze do try catch
        if (attachments.Any())
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

    private readonly ILogger<DeleteRealEstateValuationHandler> _logger;
    private readonly ExternalServices.PreorderService.V1.IPreorderServiceClient _preorderService;
    private readonly RealEstateValuationServiceDbContext _dbContext;

    public DeleteRealEstateValuationHandler(
        RealEstateValuationServiceDbContext dbContext, 
        ILogger<DeleteRealEstateValuationHandler> logger, 
        ExternalServices.PreorderService.V1.IPreorderServiceClient preorderService)
    {
        _preorderService = preorderService;
        _dbContext = dbContext;
        _logger = logger;
    }
}
