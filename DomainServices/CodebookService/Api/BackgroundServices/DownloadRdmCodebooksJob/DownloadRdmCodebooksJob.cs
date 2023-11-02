using CIS.Core.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

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
                _logger.RdmCodebookLoading(codebook.CodebookName);

                try
                {
                   var rows = codebook.IsMapping 
                        ? await loadMapping(codebook.CodebookName, cancellationToken) 
                        : await loadCodebook(codebook.CodebookName, codebook.Fields, cancellationToken);

                    _logger.RdmCodebookLoaded(codebook.CodebookName, rows);
                }
                catch (Exception ex)
                {
                    _logger.RdmCodebookLoadingException(codebook.CodebookName, ex);
                }
            }
        }
    }

    private async Task<int> loadMapping(string codebookName, CancellationToken cancellationToken)
    {
        var items = await _rdmClient.GetMappingItems(codebookName, cancellationToken);

        var dataToInsert = new List<object>(items.Count);
        foreach (var item in items)
        {
            var propsString = System.Text.Json.JsonSerializer.Serialize(new KeyValuePair<string, string>(item.CodebookEntrySource.Code, item.CodebookEntryTarget.Code));
            dataToInsert.Add(new { IsValid = item.CodebookEntryMappingProperty.State == "ACTIVE", Props = propsString });
        }

        using (var connection = _dbContext.Create())
        {
            connection.Open();

            await connection.ExecuteAsync($"DELETE FROM dbo.RdmCodebook WHERE RdmCodebookName='{codebookName}'");
            await connection.ExecuteAsync($"INSERT INTO dbo.RdmCodebook (RdmCodebookName, EntryIsValid, EntryProperties) VALUES ('{codebookName}', @IsValid, @Props)", dataToInsert);
        }

        return items?.Count ?? 0;
    }

    private async Task<int> loadCodebook(string codebookName, string[]? fields, CancellationToken cancellationToken)
    {
        var items = await _rdmClient.GetCodebookItems(codebookName, cancellationToken);

        var dataToInsert = new List<object>(items.Count);
        foreach (var item in items)
        {
            var values = item.CodebookEntryValues;
            if (fields?.Any() ?? false)
            {
                values = values.Where(t => fields.Contains(t.CodebookColumn.Code)).ToList();
            }

            var props = values.Select(t => new KeyValuePair<string, string>(t.CodebookColumn.Code, t.CodebookEntryValueLanguageMutations.First().MutationValue!));
            var propsString = System.Text.Json.JsonSerializer.Serialize(props);
            dataToInsert.Add(new { item.Code, IsValid = item.State == "ACTIVE", Props = propsString });
        }

        using (var connection = _dbContext.Create())
        {
            connection.Open();

            await connection.ExecuteAsync($"DELETE FROM dbo.RdmCodebook WHERE RdmCodebookName='{codebookName}'");
            await connection.ExecuteAsync($"INSERT INTO dbo.RdmCodebook (RdmCodebookName, EntryCode, EntryIsValid, EntryProperties) VALUES ('{codebookName}', @Code, @IsValid, @Props)", dataToInsert);
        }

        return items?.Count ?? 0;
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
