namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class PaginationResponse
{
    public Core.Types.PaginableResponse WithSortFields(List<(string ClientField, string ServerField)> fields)
    {
        return new Core.Types.PaginableResponse
        {
            RecordsTotalSize = this.RecordsTotalSize,
            PageSize = this.PageSize,
            RecordOffset = this.RecordOffset,
            Sort = this.Sorting.Any() ? getSortFields() : null
        };

        List<Core.Types.PaginableSortingField> getSortFields()
        {
            return this.Sorting.Select(t => new Core.Types.PaginableSortingField(getFieldName(t.Field), t.Descending)).ToList();
        }

        string getFieldName(string serverField)
        {
            var field = fields.FirstOrDefault(t => t.ServerField == serverField);
            return fields.Any(t => t.ServerField == serverField) ? fields.First(t => t.ServerField == serverField).ClientField : serverField;
        }
    }

    public static implicit operator Core.Types.PaginableResponse(PaginationResponse response)
    {
        return new Core.Types.PaginableResponse
        {
            RecordsTotalSize = response.RecordsTotalSize,
            PageSize = response.PageSize,
            RecordOffset = response.RecordOffset,
            Sort = response.Sorting.Any() ? response.Sorting.Select(t => new Core.Types.PaginableSortingField(t.Field, t.Descending)).ToList() : null,
        };
    }
}
