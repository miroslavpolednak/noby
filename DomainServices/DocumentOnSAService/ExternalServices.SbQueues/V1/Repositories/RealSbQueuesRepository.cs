using System.Data;
using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;
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
        WHERE atch.ATTACHMENT_ID = @AttachmentId AND atch.CONSUMER = 'NOBY'
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
        WHERE atch.ATTACHMENT_ID = @AttachmentId AND atch.CONSUMER = 'NOBY'
        """;

    private const string _documentSqlQuery =
      """
        SELECT  
        di.DOCUMENT_ID AS DocumentId,
        di.DOCUMENT_NAME + '.pdf' AS [FileName],
        'application/pdf' AS ContentType,
        di.DOCUMENT_FILE AS Content,
        di.FLAG_SEND_TO_PREVIEW AS IsCustomerPreviewSendingAllowed
        FROM SB_DOCUMENT_INFO_S di
        WHERE di.DOCUMENT_ID = @DocumentId AND di.CONSUMER = 'NOBY'
        """;

    private const string _documentSqlQueryWithoutContent =
        """
        SELECT  
        di.DOCUMENT_ID AS DocumentId,
        di.DOCUMENT_NAME + '.pdf' AS [FileName],
        'application/pdf' AS ContentType,
        NULL AS Content,
        di.FLAG_SEND_TO_PREVIEW AS IsCustomerPreviewSendingAllowed
        FROM SB_DOCUMENT_INFO_S di
        WHERE di.DOCUMENT_ID = @DocumentId AND di.CONSUMER = 'NOBY'
        """;

    private readonly IConnectionProvider<ISbQueuesDapperConnectionProvider> _connectionProvider;

    public RealSbQueuesRepository(IConnectionProvider<ISbQueuesDapperConnectionProvider> connectionProvider)
    {
        SqlMapper.Settings.CommandTimeout = 10;
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
}