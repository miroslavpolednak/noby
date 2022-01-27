namespace CIS.Core.Types;

public interface IPaginableResponse
{
    int PageSize { get; }
    int RecordOffset { get; }
    int RecordsTotalSize { get; }
}
