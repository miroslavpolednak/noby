using System.Net;
using System.Net.Http.Json;
using CIS.Infrastructure.Messaging.Configuration;
using Confluent.SchemaRegistry;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration.SchemaRegistry;

internal sealed class ApicurioSchemaRegistryClient : ISchemaRegistryClient
{
    private const string DefaultGroupName = "default";

    private readonly KafkaFlowConfiguration.SchemaRegistryConfiguration _schemaRegistryConfiguration;
    private readonly HttpClient _httpClient;

    public ApicurioSchemaRegistryClient(KafkaFlowConfiguration.SchemaRegistryConfiguration schemaRegistryConfiguration, HttpClient httpClient)
    {
        _schemaRegistryConfiguration = schemaRegistryConfiguration;
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri(_schemaRegistryConfiguration.SchemaRegistryUrl);
    }

    public int MaxCachedSchemas => 100;

    public Task<int> GetSchemaIdAsync(string subject, string avroSchema, bool normalize = false)
    {
        return GetSchemaIdAsync(subject, new Schema(avroSchema, SchemaType.Avro), normalize);
    }

    public async Task<int> GetSchemaIdAsync(string subject, Schema schema, bool normalize = false)
    {
        try
        {
            var request = new Uri($"apis/registry/v2/groups/{DefaultGroupName}/artifacts/{WebUtility.UrlEncode(subject)}/meta", UriKind.Relative);

            var response = await _httpClient.GetAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new SchemaRegistryException($"Retrieving metadata for schema with subject '{subject}' failed", response.StatusCode, (int)response.StatusCode);

            var metadata = await response.Content.ReadFromJsonAsync<SchemaMetadata>();

            return _schemaRegistryConfiguration.SchemaIdentificationType == SchemaIdentificationType.GlobalId ? metadata!.GlobalId : metadata!.ContentId;
        }
        catch (Exception ex)
        {
            throw new SchemaRegistryException($"Retrieving schema threw exception with message: {ex.Message}", HttpStatusCode.InternalServerError, (int)HttpStatusCode.InternalServerError); 
        }
    }

    public async Task<Schema> GetSchemaAsync(int id, string? format = null)
    {
        try
        {
            var request = new Uri($"apis/registry/v2/ids/contentIds/{id}", UriKind.Relative);

            var response = await _httpClient.GetAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new SchemaRegistryException($"Retrieving schema with id '{id}' failed", response.StatusCode, (int)response.StatusCode);

            var schemaString = await response.Content.ReadAsStringAsync();

            return new Schema(schemaString, SchemaType.Avro);
        }
        catch (Exception ex)
        {
            throw new SchemaRegistryException($"Retrieving schema threw exception with message: {ex.Message}", HttpStatusCode.InternalServerError, (int)HttpStatusCode.InternalServerError);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _httpClient.Dispose();
    }

    public Task<RegisteredSchema> GetRegisteredSchemaAsync(string subject, int version) => throw new NotImplementedException();

    public Task<List<int>> GetSubjectVersionsAsync(string subject) => throw new NotImplementedException();

    public Task<string> GetSchemaAsync(string subject, int version) => throw new NotSupportedException();

    public Task<int> RegisterSchemaAsync(string subject, string avroSchema, bool normalize = false) => throw new NotSupportedException();

    public Task<int> RegisterSchemaAsync(string subject, Schema schema, bool normalize = false) => throw new NotSupportedException();

    public Task<bool> IsCompatibleAsync(string subject, string avroSchema) => throw new NotSupportedException();

    public Task<bool> IsCompatibleAsync(string subject, Schema schema) => throw new NotSupportedException();

    public Task<RegisteredSchema> GetLatestSchemaAsync(string subject) => throw new NotSupportedException();

    public Task<List<string>> GetAllSubjectsAsync() => throw new NotSupportedException();

    public Task<RegisteredSchema> LookupSchemaAsync(string subject, Schema schema, bool ignoreDeletedSchemas, bool normalize = false) => throw new NotSupportedException();

    public string ConstructKeySubjectName(string topic, string? recordType = null) => throw new NotSupportedException();

    public string ConstructValueSubjectName(string topic, string? recordType = null) => throw new NotSupportedException();
}