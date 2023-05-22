using Newtonsoft.Json;

namespace DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Dto;

public class IdentifiedSubjectError
{
    [JsonProperty(PropertyName = "message")]
    public required string Message { get; init; }

    [JsonProperty(PropertyName = "detail")]
    public required string[] Detail { get; init; }
}