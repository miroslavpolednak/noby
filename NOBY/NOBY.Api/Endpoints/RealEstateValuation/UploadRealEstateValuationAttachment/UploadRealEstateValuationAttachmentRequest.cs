namespace NOBY.Api.Endpoints.RealEstateValuation.CreateRealEstateValuationAttachment;

internal sealed class UploadRealEstateValuationAttachmentRequest
    : IRequest<Guid>
{
    public long CaseId { get; set; }

    public int RealEstateValuationId { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public IFormFile File { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
