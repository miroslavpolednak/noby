using CIS.InternalServices.DataAggregator.Configuration;

namespace CIS.InternalServices.DataAggregator.Helpers;

internal static class CollectionPathHelper
{
    public static string GetCollectionPath(string path)
    {
        var splitPath = path.Split(ConfigurationConstants.CollectionMarker);

        return splitPath.Length == 1 ? string.Empty : splitPath.First();
    }

    public static string GetCollectionMemberPath(string path, int skip = 0) => 
        path.Split(ConfigurationConstants.CollectionMarker, 2 + skip).Last().TrimStart('.');
}