﻿using CIS.Core.Exceptions;
using CIS.Core.Exceptions.ExternalServices;
using Microsoft.Extensions.Logging;

namespace DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Clients;
public class TcpClient : ITcpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TcpClient> _logger;

    public TcpClient(
        HttpClient httpClient,
        ILogger<TcpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<byte[]> DownloadFile(string url, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync(url, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsByteArrayAsync(cancellationToken);
        }
        else
        {
            var result = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.ErrorWhenDownloadingFile(result);
            
            if (result.Contains("item not found"))
            {
                throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.TcpDocumentNotExistOnUrl);
            }
            else
            {
                throw new CisExternalServiceServerErrorException("eArchive(TCP)", nameof(DownloadFile), result);
            }
        }
    }
}
