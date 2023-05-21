using Newtonsoft.Json;

namespace DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Dto;

public class IdentifiedSubjectValidationError
{
    [JsonProperty(PropertyName = "message")]
    public string? Message { get; init; }

    [JsonProperty(PropertyName = "detail")]
    public required ErrorDetail Detail { get; init; }

    public class ErrorDetail
    {
        [JsonProperty(PropertyName = "violatedConstraints")]
        public ViolatedConstraint[]? ViolatedConstraints { get; init; }
    }

    public class ViolatedConstraint
    {
        [JsonProperty(PropertyName = "attribute")]
        public string? Attribute { get; init; }

        [JsonProperty(PropertyName = "invalidValue", Required = Required.Default)]
        public string? InvalidValue { get; init; }

        [JsonProperty(PropertyName = "message")]
        public string? Message { get; init; }
    }
}