namespace CIS.InternalServices.DocumentDataAggregator.Mapper;

public static class CollectionPathHelper
{
    public static string GetCollectionPath(string path)
    {
        var splitPath = path.Split(DocumentConfiguration.FieldPathCollectionMarker);

        return splitPath.Length == 1 ? string.Empty : splitPath.First();
    }

    public static string GetCollectionMemberPath(string path) => 
        path.Split(DocumentConfiguration.FieldPathCollectionMarker).Last().TrimStart('.');
}