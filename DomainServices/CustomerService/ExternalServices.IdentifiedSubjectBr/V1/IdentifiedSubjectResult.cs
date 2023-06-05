using DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Dto;

namespace DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1;

public class IdentifiedSubjectResult<TResult>
{
    public TResult? Result { get; init; }

    public IdentifiedSubjectError? Error { get; init; }
}