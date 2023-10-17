using CIS.Core.Data;

namespace DomainServices.CodebookService.Api.BackgroundServices.DownloadRdmCodebooksJob;

internal sealed class DownloadRdmCodebooksJob
    : CIS.Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        if (_appConfiguration.RdmCodebooksToUpdate?.Any() ?? false)
        {
            foreach (var codebook in _appConfiguration.RdmCodebooksToUpdate)
            {
                var items = await _rdmClient.GetCodebookItems(codebook.CodebookName, cancellationToken);
            }
        }
    }

    private readonly ExternalServices.RDM.V1.IRDMClient _rdmClient;
    private readonly Configuration.AppConfiguration _appConfiguration;
    private readonly IConnectionProvider _dbContext;
    private readonly ILogger<DownloadRdmCodebooksJob> _logger;

    public DownloadRdmCodebooksJob(
        ExternalServices.RDM.V1.IRDMClient rdmClient,
        Configuration.AppConfiguration appConfiguration,
        IConnectionProvider dbContext,
        ILogger<DownloadRdmCodebooksJob> logger)
    {
        _rdmClient = rdmClient;
        _appConfiguration = appConfiguration;
        _dbContext = dbContext;
        _logger = logger;
    }
}
