using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using Google.Protobuf;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.CreateRealEstateValuationAttachment;

internal sealed class CreateRealEstateValuationAttachmentHandler
    : IRequestHandler<CreateRealEstateValuationAttachmentRequest, CreateRealEstateValuationAttachmentResponse>
{
    public async Task<CreateRealEstateValuationAttachmentResponse> Handle(CreateRealEstateValuationAttachmentRequest request, CancellationToken cancellationToken)
    {
        if (!(await _dbContext
            .RealEstateValuations
            .AnyAsync(t => t.RealEstateValuationId == request.RealEstateValuationId, cancellationToken)))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);
        }

        // upload do ACV
        var result = await _preorderService.UploadAttachment(request.Title, request.FileName, request.MimeType, request.FileData.ToArray(), cancellationToken);

        // ulozeni u nas
        var entity = new Database.Entities.RealEstateValuationAttachment
        {
            RealEstateValuationId = request.RealEstateValuationId,
            FileName = request.FileName,
            ExternalId = result,
            Title = request.Title
        };
        _dbContext.Attachments.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateRealEstateValuationAttachmentResponse
        {
            RealEstateValuationAttachmentId = entity.RealEstateValuationAttachmentId
        };
    }

    private readonly ExternalServices.PreorderService.V1.IPreorderServiceClient _preorderService;
    private readonly RealEstateValuationServiceDbContext _dbContext;

    public CreateRealEstateValuationAttachmentHandler(
        RealEstateValuationServiceDbContext dbContext, 
        ExternalServices.PreorderService.V1.IPreorderServiceClient preorderService)
    {
        _dbContext = dbContext;
        _preorderService = preorderService;
    }
}
