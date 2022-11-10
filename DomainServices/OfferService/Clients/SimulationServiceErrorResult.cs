namespace DomainServices.OfferService.Clients;

public sealed class SimulationServiceErrorResult : CIS.Core.Results.IServiceCallResult
{
    public IReadOnlyCollection<(string Key, string Message)> Errors { get; init; }
    public bool Success => false;

    public SimulationServiceErrorResult(string? key, string? message)
    {
        //if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

        Errors = (new List<(string Key, string Message)>(1) { (key ?? "", message ?? "") }).AsReadOnly();
    }
}
