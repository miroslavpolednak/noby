namespace DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Dto;

public class IdentifiedSubjectValidationError
{
    public required string Message { get; init; }

    public required ErrorDetail Detail { get; init; }

    public class ErrorDetail
    {
        public ViolatedConstraint[]? ViolatedConstraints { get; init; }
    }

    public class ViolatedConstraint
    {
        public string? Attribute { get; init; }

        public string? InvalidValue { get; init; }

        public string? Message { get; init; }
    }
}