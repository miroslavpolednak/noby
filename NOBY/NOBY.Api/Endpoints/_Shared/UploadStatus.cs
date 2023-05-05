namespace NOBY.Api.Endpoints.Shared;

public class UploadStatusHelper
{
    public static UploadStatus GetUploadStatus(int stateInQueue) => stateInQueue switch
    {
        100 or 110 or 200 => UploadStatus.InProgress,
        300 => UploadStatus.Error,
        400 => UploadStatus.SaveInEArchive,
        _ => throw new ArgumentException("StatusInDocumentInterface is not supported")
    };
}

public enum UploadStatus
{
    SaveInEArchive = 0,
    InProgress = 1,
    Error = 2
}
