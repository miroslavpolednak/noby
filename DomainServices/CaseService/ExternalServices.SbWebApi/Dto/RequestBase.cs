namespace DomainServices.CaseService.ExternalServices.SbWebApi.Dto;

public abstract class RequestBase
{
    public required string HeaderLogin { get; init; }
}