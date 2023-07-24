using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

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

        var category = (await _codebookService.AcvAttachmentCategories(cancellationToken))
            .First(t => t.Id == request.AcvAttachmentCategoryId)
            .Code;

        // upload do ACV
        var result = await _preorderService.UploadAttachment(request.Title, category, request.FileName, request.MimeType, request.FileData.ToArray(), cancellationToken);

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

    private DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly ExternalServices.PreorderService.V1.IPreorderServiceClient _preorderService;
    private readonly RealEstateValuationServiceDbContext _dbContext;

    public CreateRealEstateValuationAttachmentHandler(
        RealEstateValuationServiceDbContext dbContext, 
        ExternalServices.PreorderService.V1.IPreorderServiceClient preorderService,
        CodebookService.Clients.ICodebookServiceClient codebookService)
    {
        _dbContext = dbContext;
        _preorderService = preorderService;
        _codebookService = codebookService;
    }
}
