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
        var model = new PaginationResponse
        {
            RecordsTotalSize = recordsTotalSize,
            PageSize = this.PageSize,
            RecordOffset = this.RecordOffset,
        };
        if (this.Sorting.Any())
            model.Sorting.AddRange(this.Sorting);
        return model;
    }

    public static implicit operator PaginationRequest(Core.Types.PaginableRequest request)
    {
        var model = new PaginationRequest
        {
            PageSize = request.PageSize,
            RecordOffset = request.RecordOffset
        };
        if (request.Sort is not null && request.Sort.Any())
            model.Sorting.AddRange(request.Sort.Select(t => new PaginationSortingField { Descending = t.Descending, Field = t.Field }));
        
        return model;
    }
}
