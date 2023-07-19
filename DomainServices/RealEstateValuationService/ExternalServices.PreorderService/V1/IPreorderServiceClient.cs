using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;

public interface IPreorderServiceClient
    : IExternalServiceClient
{
    const string Version = "V1";

    Task<long> UploadAttachment(string title, string fileName, string mimeType, byte[] fileData, CancellationToken cancellationToken = default);
}
