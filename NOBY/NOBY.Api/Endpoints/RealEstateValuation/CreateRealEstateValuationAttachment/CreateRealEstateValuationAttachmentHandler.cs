using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.CreateRealEstateValuationAttachment;

internal sealed class CreateRealEstateValuationAttachmentHandler
    : IRequestHandler<CreateRealEstateValuationAttachmentRequest, int>
{
    public async Task<int> Handle(CreateRealEstateValuationAttachmentRequest request, CancellationToken cancellationToken)
    {
        var dsRequest = new DomainServices.RealEstateValuationService.Contracts.CreateRealEstateValuationAttachmentRequest
        {
            RealEstateValuationId = request.RealEstateValuationId,
            FileName = request.File!.FileName,
            MimeType = request.File.ContentType,
            Title = request.Title,
            FileData = Google.Protobuf.ByteString.FromStream(request.File.OpenReadStream())
        };

        return await _realEstateValuationService.CreateRealEstateValuationAttachment(dsRequest, cancellationToken);
    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public CreateRealEstateValuationAttachmentHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
