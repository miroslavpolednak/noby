namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class PaginationResponse
{
    public static implicit operator Core.Types.PaginableResponse(PaginationResponse response)
    {
        return new Core.Types.PaginableResponse
        {
            RecordsTotalSize = response.RecordsTotalSize,
            PageSize = response.PageSize,
            RecordOffset = response.RecordOffset
        };
    }
}
