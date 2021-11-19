namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class PaginationRequest
{
    public static PaginationRequest ParseOrDefault(PaginationRequest request, int defaultPageSize = 10, int defaultRecordOffset = 1)
    {
        if (request == null || request.PageSize <= 0 || request.RecordOffset < 0)
            return new PaginationRequest() { PageSize = defaultPageSize, RecordOffset = defaultRecordOffset };
        return request;
    }

    public PaginationResponse CreateResponse()
    {
        return new PaginationResponse
        {
            PageSize = this.PageSize,
            RecordOffset = this.RecordOffset
        };
    }

    public PaginationResponse CreateResponse(int recordsTotalSize)
    {
        return new PaginationResponse
        {
            RecordsTotalSize = recordsTotalSize,
            PageSize = this.PageSize,
            RecordOffset = this.RecordOffset
        };
    }
}
