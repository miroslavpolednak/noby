using System.Text.Json.Serialization;
using Confluent.SchemaRegistry;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration.SchemaRegistry;

internal class SchemaMetadata
{
    [JsonPropertyName("groupId")]
    public string? GroupId { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type"), JsonConverter(typeof(SchemaTypeConverter))]
    public SchemaType Type { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("globalId")]
    public int GlobalId { get; set; }

    [JsonPropertyName("contentId")]
    public int ContentId { get; set; }
}