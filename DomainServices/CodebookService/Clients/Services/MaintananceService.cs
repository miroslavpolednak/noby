namespace DomainServices.CodebookService.Clients.Services;

internal sealed class MaintananceService
    : IMaintananceService
{
    public async Task DownloadRdmCodebooks(List<string> codebookNames, CancellationToken cancellationToken = default)
    {
        var request = new Contracts.DownloadRdmCodebooksRequest();
        request.CodebookNames.AddRange(codebookNames);
        await _service.DownloadRdmCodebooksAsync(request, cancellationToken: cancellationToken);
    }
    
    private readonly Contracts.MaintananceService.MaintananceServiceClient _service;

    public MaintananceService(Contracts.MaintananceService.MaintananceServiceClient service)
    {
        _service = service;
    }
}
