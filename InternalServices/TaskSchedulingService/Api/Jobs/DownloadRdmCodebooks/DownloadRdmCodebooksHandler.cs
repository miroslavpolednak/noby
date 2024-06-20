using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.CodebookService.Clients;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.DownloadRdmCodebooks;

/// <summary>
/// JobData: [string] where values are codebook names
/// </summary>
internal sealed class DownloadRdmCodebooksHandler(IMaintananceService _client)
    : IJob
{
    public async Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(jobData))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.DownloadRdmCodebooksNamesMissing);
        }

        var codebooks = System.Text.Json.JsonSerializer.Deserialize<List<string>>(jobData);
        if (codebooks is null || codebooks.Count == 0)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.DownloadRdmCodebooksNamesMissing);
        }

        await _client.DownloadRdmCodebooks(codebooks, cancellationToken);
    }
}
