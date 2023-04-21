namespace DomainServices.CaseService.ExternalServices.SbWebApi.Dto;

public abstract class RequestBase
{
    /// <summary>
    /// Login uživatele - ČPM.
    /// </summary>
    public string Login { get; set; } = string.Empty;
}