using Google.Protobuf.Collections;

namespace DomainServices.CodebookService.Contracts;

public interface IItemsResponse<TItem>
    where TItem : class
{
    RepeatedField<TItem> Items { get; }
}

public interface IBaseCodebook
{
    int Id { get; set; }
    string Name { get; set; }
    bool? IsValid { get; set; }
}