namespace ExternalServices.ESignatureQueues.V1.Repositories;

public class MockESignatureQueuesRepository : IESignatureQueuesRepository
{
    public async Task<string> GetAttachmentExternalId(string attachmentId, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        return Guid.NewGuid().ToString();
    }
}