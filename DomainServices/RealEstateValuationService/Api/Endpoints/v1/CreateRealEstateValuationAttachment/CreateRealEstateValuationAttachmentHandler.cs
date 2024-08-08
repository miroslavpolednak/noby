using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.CreateRealEstateValuationAttachment;

internal sealed class CreateRealEstateValuationAttachmentHandler(
    RealEstateValuationServiceDbContext _dbContext,
    ExternalServices.PreorderService.V1.IPreorderServiceClient _preorderService,
    CodebookService.Clients.ICodebookServiceClient _codebookService)
        : IRequestHandler<CreateRealEstateValuationAttachmentRequest, CreateRealEstateValuationAttachmentResponse>
{
    public async Task<CreateRealEstateValuationAttachmentResponse> Handle(CreateRealEstateValuationAttachmentRequest request, CancellationToken cancellationToken)
    {
        if (!await _dbContext
            .RealEstateValuations
            .AnyAsync(t => t.RealEstateValuationId == request.RealEstateValuationId, cancellationToken))
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);
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
            Title = request.Title,
            AcvAttachmentCategoryId = request.AcvAttachmentCategoryId
        };
        _dbContext.Attachments.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateRealEstateValuationAttachmentResponse
        {
            RealEstateValuationAttachmentId = entity.RealEstateValuationAttachmentId
        };
    }
}
