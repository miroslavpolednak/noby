namespace CIS.Core.Types;

public class PaginableResponse
{
	public int RecordOffset { get; set; }
	public int PageSize { get; set; }
	public int RecordsTotalSize { get; set; }
}
