namespace DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Dto;

public class IdentifiedSubjectError
{
    public required string Message { get; init; }

    public required string[] Detail { get; init; }
}