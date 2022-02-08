namespace FOMS.Api.SharedHandlers.Requests;

internal sealed class SharedCreateProductRequest
    : IRequest<long>
{
    public long CaseId { get; set; }
    public int ProductTypeId { get; set; }
}
