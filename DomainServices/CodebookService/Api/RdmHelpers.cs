using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;

namespace DomainServices.CodebookService.Api;

internal static class RdmHelpers
{
    public static List<RdmCodebookItem> GetRdmItems(this IConnectionProvider connectionProvider, string codebookName)
    {
        using var connection = connectionProvider.Create();
        connection.Open();
        var list = connectionProvider.ExecuteDapperQuery(c =>
        {
            return c.Query("SELECT EntryIsValid, EntryCode, EntryProperties, SortOrder FROM dbo.RdmCodebook WHERE RdmCodebookName=@name", new { name = codebookName }).ToList();
        });

        return list
            .Select(t =>
            {
                var d = System.Text.Json.JsonSerializer.Deserialize<KeyValuePair<string, string>[]>(t.EntryProperties);
                return new RdmCodebookItem
                {
                    Code = t.EntryCode,
                    IsValid = t.EntryIsValid,
                    Properties = ((KeyValuePair<string, string>[])d).ToDictionary(k => k.Key, v => v.Value)
                };
            })
            .ToList();
    }

    public static List<RdmMappingItem> GetRdmMappings(this IConnectionProvider connectionProvider, string codebookName)
    {
        using var connection = connectionProvider.Create();
        connection.Open();
        var list = connectionProvider.ExecuteDapperQuery(c => 
        {
            return c.Query<string>("SELECT EntryProperties FROM dbo.RdmCodebook WHERE RdmCodebookName=@name AND EntryIsValid=1", new { name = codebookName }).ToList();
        });

        return list
            .Select(t =>
            {
                var d = System.Text.Json.JsonSerializer.Deserialize<KeyValuePair<string, string>>(t);
                return new RdmMappingItem
                {
                    Source = d.Key,
                    Target = d.Value
                };
            })
            .ToList();
    }

    public sealed class RdmCodebookItem
    {
        public string Code { get; set; } = string.Empty;
        public bool IsValid { get; set; }
        public Dictionary<string, string> Properties { get; set; } = null!;
        public int SortOrder { get; set; }
    }

    public sealed class RdmMappingItem
    {
        public string Source { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
    }
}
