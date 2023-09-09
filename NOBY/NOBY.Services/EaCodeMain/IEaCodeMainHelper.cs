namespace NOBY.Services.EaCodeMain;

public interface IEaCodeMainHelper
{
    Task ValidateEaCodeMain(int eCodeMainId, CancellationToken cancellationToken);

    Task<List<int>> GetDocumentTypeIdsAccordingEaCodeMain(int eCodeMainId, CancellationToken cancellationToken);
}
