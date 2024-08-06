using System.Data;
using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;
using DomainServices.DocumentOnSAService.ExternalServices.SbQueues.V1.Repositories;
using ExternalServices.SbQueues.Abstraction;
using ExternalServices.SbQueues.V1.Model;

namespace ExternalServices.SbQueues.V1.Repositories;

public class RealSbQueuesRepository : ISbQueuesRepository
{
    private const string _attachmentSqlQuery =
       """
        SELECT 
        atch.ATTACHMENT_ID AS AttachmentId, 
        atch.[FILE_NAME] AS [FileName],
        'application/pdf' AS ContentType,
        atch.ATTACHMENT_FILE AS Content,
        atch.FLAG_SEND_TO_PREVIEW AS IsCustomerPreviewSendingAllowed
        FROM SB_ATTACHMENT_INFO_S atch 
        WHERE atch.ATTACHMENT_ID = @AttachmentId
        """;

    private const string _attachmentSqlQueryWithoutContent =
        """
        SELECT 
        atch.ATTACHMENT_ID AS AttachmentId, 
        atch.[FILE_NAME] AS [FileName],
        'application/pdf' AS ContentType,
        NULL AS Content,
        atch.FLAG_SEND_TO_PREVIEW AS IsCustomerPreviewSendingAllowed
        FROM SB_ATTACHMENT_INFO_S atch 
        WHERE atch.ATTACHMENT_ID = @AttachmentId'
        """;

    private const string _documentSqlQuery =
      """
         SELECT  
         di.DOCUMENT_ID AS DocumentId,
         di.DOCUMENT_NAME + '.pdf' AS [FileName],
         'application/pdf' AS ContentType,
         di.DOCUMENT_FILE AS Content,
         di.FLAG_SEND_TO_PREVIEW AS IsCustomerPreviewSendingAllowed,
         di.EXTERNAL_ID AS ExternalIdESignatures
         FROM SB_DOCUMENT_INFO_S di
         WHERE di.DOCUMENT_ID = @DocumentId
        """;

    private const string _documentSqlQueryWithoutContent =
        """
        SELECT  
        di.DOCUMENT_ID AS DocumentId,
        di.DOCUMENT_NAME + '.pdf' AS [FileName],
        'application/pdf' AS ContentType,
        NULL AS Content,
        di.FLAG_SEND_TO_PREVIEW AS IsCustomerPreviewSendingAllowed,
        di.EXTERNAL_ID AS ExternalIdESignatures
        FROM SB_DOCUMENT_INFO_S di
        WHERE di.DOCUMENT_ID = @DocumentId
        """;

    private const string _updateAttachmentProcessingDateSql =
        """
           UPDATE dbo.SB_ATTACHMENT_INFO_S 
           SET DATUM_ZPRACOVANIA = @DateTime 
           WHERE ATTACHMENT_ID = @AttachmentId AND CONSUMER = 'NOBY' 
           """;

    public const string _updateDocumentProcessingDateSql =
        """
            UPDATE dbo.SB_DOCUMENT_INFO_S 
            SET DATUM_ZPRACOVANIA = @DateTime 
            WHERE DOCUMENT_ID = @DocumentId AND CONSUMER = 'NOBY'
            """;

    public const string _updateClientProcessingDateSql =
        """
            UPDATE dbo.SB_CLIENT_INFO_S 
            SET DATUM_ZPRACOVANIA = @DateTime
            WHERE DOCUMENT_ID = @DocumentId AND CONSUMER = 'NOBY'
            """;

    public const string _getDocumentIdSql =
        """
        SELECT A.DOCUMENT_ID 
        FROM dbo.SB_ATTACHMENT_INFO_S A 
        WHERE A.ATTACHMENT_ID = @AttachmentId
        """;

    private readonly IConnectionProvider<ISbQueuesDapperConnectionProvider> _connectionProvider;

    public RealSbQueuesRepository(IConnectionProvider<ISbQueuesDapperConnectionProvider> connectionProvider)
    {
        SqlMapper.Settings.CommandTimeout = 60;
        _connectionProvider = connectionProvider;
    }

    public async Task<Attachment?> GetAttachmentById(string attachmentId, bool getMetadataOnly, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@AttachmentId", attachmentId, DbType.Int64, ParameterDirection.Input);

        if (getMetadataOnly)
        {
            return await _connectionProvider
                          .ExecuteDapperFirstOrDefaultAsync<Attachment>(_attachmentSqlQueryWithoutContent, parameters, cancellationToken);
        }
        else
        {
            return await _connectionProvider
                          .ExecuteDapperFirstOrDefaultAsync<Attachment>(_attachmentSqlQuery, parameters, cancellationToken);
        }
    }

    public async Task<Document?> GetDocumentByExternalId(string documentId, bool getMetadataOnly, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@DocumentId", documentId, DbType.Int64, ParameterDirection.Input);

        if (getMetadataOnly)
        {
            return await _connectionProvider
           .ExecuteDapperFirstOrDefaultAsync<Document>(_documentSqlQueryWithoutContent, parameters, cancellationToken);
        }
        else
        {
            return await _connectionProvider
            .ExecuteDapperFirstOrDefaultAsync<Document>(_documentSqlQuery, parameters, cancellationToken);
        }
    }

    public async Task UpdateAttachmentProcessingDate(long attachmentId, DateTime? processingDate, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@AttachmentId", attachmentId, DbType.Int64, ParameterDirection.Input);
        parameters.Add("@DateTime", processingDate, DbType.DateTime, ParameterDirection.Input);

        await _connectionProvider.ExecuteDapperQueryAsync(async c => await c.ExecuteAsync(_updateAttachmentProcessingDateSql, parameters), cancellationToken);
    }

    public async Task UpdateClientProcessingDate(long documentId, DateTime? processingDate, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@DocumentId", documentId, DbType.Int64, ParameterDirection.Input);
        parameters.Add("@DateTime", processingDate, DbType.DateTime, ParameterDirection.Input);

        await _connectionProvider.ExecuteDapperQueryAsync(async c => await c.ExecuteAsync(_updateClientProcessingDateSql, parameters), cancellationToken);

    }

    public async Task UpdateDocumentProcessingDate(long documentId, DateTime? processingDate, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@DocumentId", documentId, DbType.Int64, ParameterDirection.Input);
        parameters.Add("@DateTime", processingDate, DbType.DateTime, ParameterDirection.Input);

        await _connectionProvider.ExecuteDapperQueryAsync(async c => await c.ExecuteAsync(_updateDocumentProcessingDateSql, parameters), cancellationToken);
    }

    public async Task<long> GetDocumentIdAccordingAtchId(string attachmentId, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@AttachmentId", attachmentId, DbType.Int64, ParameterDirection.Input);

        return await _connectionProvider
                         .ExecuteDapperFirstOrDefaultAsync<long>(_getDocumentIdSql, parameters, cancellationToken);
    }
}