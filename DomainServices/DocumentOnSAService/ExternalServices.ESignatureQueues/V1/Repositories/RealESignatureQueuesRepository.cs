using System.Data;
using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;
using ExternalServices.ESignatureQueues.Abstraction;
using ExternalServices.ESignatureQueues.V1.Model;

namespace ExternalServices.ESignatureQueues.V1.Repositories;

public class RealESignatureQueuesRepository : IESignatureQueuesRepository
{
    private const string _attachmentSqlQuery =
       """
        SELECT
            atch.[A20ID] as Id,
            atch.[A10FILE_BINARY_ID] as FileBinaryId,
            fbin.[A10NAME] as Name,
            fbin.[A10CONTENT_TYPE] as ContentType,
            fbin.[A10CONTENT] as Content
        FROM [dbo].[A20ATTACHMENT] atch
        INNER JOIN [dbo].[A10FILE_BINARY] fbin ON atch.[A10FILE_BINARY_ID] = fbin.[A10ID]
        WHERE atch.[A20ID] = @AttachmentId
        """;

    private const string _documentSqlQuery =
      """
        SELECT
            doc.[A26ID] as Id,
            doc.[A26EXTERNAL_ID] as ExternalId,
            doc.[A10FILE_BINARY_ORIGINAL_ID] as FileBinaryOriginalId,
            fbin.[A10NAME] as Name,
            fbin.[A10CONTENT_TYPE] as ContentType,
            fbin.[A10CONTENT] as Content
        FROM [dbo].[A26DOCUMENT] doc
        INNER JOIN [dbo].[A10FILE_BINARY] fbin ON doc.[A10FILE_BINARY_ORIGINAL_ID] = fbin.[A10ID]
        WHERE doc.[A26EXTERNAL_ID] = @ExternalId
        """;

    private readonly IConnectionProvider<IESignatureQueuesDapperConnectionProvider> _connectionProvider;

    public RealESignatureQueuesRepository(IConnectionProvider<IESignatureQueuesDapperConnectionProvider> connectionProvider)
    {
        SqlMapper.Settings.CommandTimeout = 10;
        _connectionProvider = connectionProvider;
    }

    public async Task<Attachment?> GetAttachmentById(long attachmentId, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@AttachmentId", attachmentId, DbType.Int64, ParameterDirection.Input);

        return await _connectionProvider
            .ExecuteDapperFirstOrDefaultAsync<Attachment>(_attachmentSqlQuery, parameters);
    }
    
    public async Task<Document?> GetDocumentByExternalId(string externalId, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@ExternalId", externalId, DbType.String, ParameterDirection.Input);

        return await _connectionProvider
            .ExecuteDapperFirstOrDefaultAsync<Document>(_documentSqlQuery, parameters);
    }
}