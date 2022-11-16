namespace CIS.InternalServices.DocumentDataAggregator.Helpers;

public static class CollectionPathHelper
{
    public static string GetCollectionPath(string path)
    {
        var splitPath = path.Split(ConfigurationConstants.CollectionMarker);

        return splitPath.Length == 1 ? string.Empty : splitPath.First();
    }

    public static string GetCollectionMemberPath(string path) => 
        path.Split(ConfigurationConstants.CollectionMarker).Last().TrimStart('.');
}