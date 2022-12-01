using CIS.Core.Data;
using CIS.Core.Exceptions;
using CIS.Infrastructure.Data;
using ExternalServicesTcp.Data;
using ExternalServicesTcp.Model;

namespace ExternalServicesTcp.V1.Repositories
{
    public class DocumentServiceRepository : IDocumentServiceRepository
    {
        private const string GetDocumentByExternalIdSql =
         """
 SELECT 
 ds.CASE_ID AS CaseId,
 ds.DOKUMENT_ID AS DocumentId,
 ds.HESLO_KOD AS EaCodeMainId,
 ds.NAZEV_SOUBORU AS Filename,
 ds.OCENENI_ID AS OrderId,
 ds.DATUM_PRIJETI AS CreatedOn,
 ds.REFERENT AS AuthorUserLogin,
 ds.PRIORITA AS Priority,
 ds.STATUS AS Status,
 ds.VAZBA_TYP AS FolderDocument,
 ds.VAZBA_ID AS FolderDocumentId,
 ds.TYP_DOKUMENTU AS DocumentDirection,
 ds.FORMULAR_ID AS FormId,
 ds.CISLO_SMLOUVY AS ContractNumber,
 ds.CISLO_ZASTAVNI_SMLOUVY AS PledgeAgreementNumber,
 ds.KOMPLETNI AS Completeness,
 fc.URL AS Url,
 fc.MIMETYPE AS MimeType,
 fc.FILENAME AS FileName,
 (
     SELECT  
     LISTAGG(dh.HESLO_KOD, ',')
     WITHIN GROUP (ORDER BY dh.HESLO_KOD)
     AS hk
     FROM "PDMS_SDILENI"."DOKUMENTY_VHESLA1" dh WHERE ds.ENTITYNUMBER = dh.ENTITYNUMBER 
     GROUP BY dh.ENTITYNUMBER
 ) AS MinorCodes  
 FROM "PDMS_SDILENI"."DOKUMENTY_DOCSERVICE" ds
 INNER JOIN pdms_sdileni.dokumenty_komp2 fc ON fc.DOKUMENT_ID = ds.NULADOKUMENT_ID
 WHERE ds.NULADOKUMENT_ID IN (concat('0',:ExternalDocumentId))
 """;
        private readonly IConnectionProvider<ITcpDapperConnectionProvider> _connectionProvider;

        public DocumentServiceRepository(IConnectionProvider<ITcpDapperConnectionProvider> connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<DocumentServiceQueryResult> GetDocumentByExternalId(GetDocumentByExternalIdTcpQuery query, CancellationToken cancellationToken)
        {
            var result = await _connectionProvider
                .ExecuteDapperRawSqlFirstOrDefault<DocumentServiceQueryResult>(
                GetDocumentByExternalIdSql,
                new { ExternalDocumentId = query.DocumentId },
                cancellationToken);

            if (result == null)
            {
                throw new CisNotFoundException(14003, "Document with ExternalId not found");
            }

            return result;
        }
    }
}
