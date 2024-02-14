namespace DomainServices.CodebookService.Clients;

public interface IMaintananceService
{
    Task DownloadRdmCodebooks(List<string> codebookNames, CancellationToken cancellationToken = default);
}
