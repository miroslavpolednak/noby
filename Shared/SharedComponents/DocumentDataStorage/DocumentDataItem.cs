using System.Globalization;

namespace SharedComponents.DocumentDataStorage;

public sealed record DocumentDataItem<TData>(int DocumentDataStorageId, int Version, TData? Data, string? EntityId)
    where TData : class, IDocumentData
{
    public int EntityIdInt { get => Convert.ToInt32(EntityId, CultureInfo.InvariantCulture); }
}
