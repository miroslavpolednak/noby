namespace FOMS.Api.SharedHandlers.Requests;

internal sealed class SharedCreateProductInstanceRequest
    : IRequest<long>
{
    public long CaseId { get; set; }
    public int ProductInstanceType { get; set; }
}
