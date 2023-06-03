using NOBY.Dto.Documents;

namespace NOBY.Api.Helpers;

internal sealed class UploadStatusHelper
{
    public static UploadStatuses GetUploadStatus(int stateInQueue) => stateInQueue switch
    {
        100 or 110 or 200 => UploadStatuses.InProgress,
        300 => UploadStatuses.Error,
        400 => UploadStatuses.SaveInEArchive,
        _ => throw new ArgumentException("StatusInDocumentInterface is not supported")
    };
}
