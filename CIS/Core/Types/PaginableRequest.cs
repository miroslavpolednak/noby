namespace CIS.Core.Types;

public record PaginableRequest (int RecordOffset, int PageSize)
{
}
