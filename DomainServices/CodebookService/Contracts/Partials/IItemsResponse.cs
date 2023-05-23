using Google.Protobuf.Collections;

namespace DomainServices.CodebookService.Contracts;

public interface IItemsResponse<TItem>
    where TItem : class
{
    RepeatedField<TItem> Items { get; }
}
