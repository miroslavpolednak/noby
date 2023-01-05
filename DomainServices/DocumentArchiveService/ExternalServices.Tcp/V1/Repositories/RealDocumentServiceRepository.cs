using CIS.Core.Data;
using CIS.Core.Exceptions;
using CIS.Infrastructure.Data;
using CIS.Infrastructure.Data.Dapper.Oracle;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.Data;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Model;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Repositories;
public class RealDocumentServiceRepository : IDocumentServiceRepository
{
    private const string DocumentMainSql = """
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
     FROM "PDMS_SDILENI"."DOKUMENTY_VHESLA1" dh WHERE ds.ENTITYNUMBER = dh.ENTITYNUMBER 
     GROUP BY dh.ENTITYNUMBER
 ) AS MinorCodes  
 FROM "PDMS_SDILENI"."DOKUMENTY_DOCSERVICE" ds
 INNER JOIN pdms_sdileni.dokumenty_komp2 fc ON fc.DOKUMENT_ID = ds.NULADOKUMENT_ID
 """;
    private const string GetDocumentByExternalIdWhereSql = "WHERE ds.NULADOKUMENT_ID IN (:ExternalDocumentId)";

    private const string FindDocumentsBaseCondition = "WHERE 1=1";

    private const string CaseIdCondition = " AND ds.NULACASE_ID = :CaseId";

    private const string AuthorUserLoginCondition = " AND ds.NULAREFERENT = :AuthorUserLogin";

    private const string CreatedOnCondition = " AND ds.DATUM_PRIJETI_FORMATED = :CreatedOn";

    private const string PledgeAgreementNumberCondition = " AND ds.NULACISLO_ZASTAVNI_SMLOUVY = :PledgeAgreementNumber";

    private const string ContractNumberCondition = " AND ds.NULACISLO_SMLOUVY = :ContractNumber";

    private const string OrderIdCondition = " AND ds.NULAOCENENI_ID = :OrderId";

    private const string FolderDocumentIdCondition = " AND ds.NULAVAZBA_ID = :FolderDocumentId";

    private static int MaxReceivedRowsCount = 5000;

    private static string MaxReceivedRows = $"FETCH NEXT {MaxReceivedRowsCount} ROWS ONLY";

    private readonly IConnectionProvider<ITcpDapperConnectionProvider> _connectionProvider;

    public RealDocumentServiceRepository(IConnectionProvider<ITcpDapperConnectionProvider> connectionProvider)
    {
        Dapper.SqlMapper.Settings.CommandTimeout = 10;
        _connectionProvider = connectionProvider;
    }

    public async Task<IReadOnlyCollection<DocumentServiceQueryResult>> FindTcpDocument(FindTcpDocumentQuery query, CancellationToken cancellationToken)
    {
        var parameters = new OracleDynamicParameters();

        var result = await _connectionProvider.ExecuteDapperRawSqlToList<DocumentServiceQueryResult>(
                                       ComposeSqlWithFilter(query, parameters),
                                       parameters,
                                       cancellationToken);

        if (result.Count >= MaxReceivedRowsCount)
        {
            throw new CisValidationException(9701, "To many results returned from external service, please specify filter more accurately.");
        }

        return result;
    }

    public async Task<DocumentServiceQueryResult> GetDocumentByExternalId(GetDocumentByExternalIdTcpQuery query, CancellationToken cancellationToken)
    {
        var parameters = new OracleDynamicParameters();
        parameters.Add("ExternalDocumentId", OracleDbType.NVarchar2, ParameterDirection.Input, $"0{query.DocumentId}");

        var result = await _connectionProvider
            .ExecuteDapperRawSqlFirstOrDefault<DocumentServiceQueryResult>(
            $"{DocumentMainSql} {GetDocumentByExternalIdWhereSql}",
            parameters,
            cancellationToken);

        if (result is null)
        {
            throw new CisNotFoundException(14002, "Unable to get/find document from eArchive (TCP)");
        }

        return result;
    }

    private string ComposeSqlWithFilter(FindTcpDocumentQuery query, OracleDynamicParameters parameters)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (parameters is null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var sb = new StringBuilder(DocumentMainSql);

        sb.Append(Environment.NewLine);
        sb.Append(FindDocumentsBaseCondition);

        if (query.CaseId is not null)
        {
            sb.Append(CaseIdCondition);
            parameters.Add("CaseId", OracleDbType.NVarchar2, ParameterDirection.Input, $"0{query.CaseId}");
        }

        if (!string.IsNullOrWhiteSpace(query.AuthorUserLogin))
        {
            sb.Append(AuthorUserLoginCondition);
            parameters.Add("AuthorUserLogin", OracleDbType.NVarchar2, ParameterDirection.Input, $"0{query.AuthorUserLogin}");
        }

        if (!string.IsNullOrWhiteSpace(query.PledgeAgreementNumber))
        {
            parameters.Add("PledgeAgreementNumber", OracleDbType.NVarchar2, ParameterDirection.Input, $"0{query.PledgeAgreementNumber}");
            sb.Append(PledgeAgreementNumberCondition);
        }

        if (!string.IsNullOrWhiteSpace(query.ContractNumber))
        {
            parameters.Add("ContractNumber", OracleDbType.NVarchar2, ParameterDirection.Input, $"0{query.ContractNumber}");
            sb.Append(ContractNumberCondition);
        }

        if (query.OrderId is not null)
        {
            parameters.Add("OrderId", OracleDbType.NVarchar2, ParameterDirection.Input, $"0{query.OrderId}");
            sb.Append(OrderIdCondition);
        }

        if (!string.IsNullOrWhiteSpace(query.FolderDocumentId))
        {
            parameters.Add("FolderDocumentId", OracleDbType.NVarchar2, ParameterDirection.Input, $"0{query.FolderDocumentId}");
            sb.Append(FolderDocumentIdCondition);
        }

        if (query.CreatedOn is not null)
        {
            parameters.Add("CreatedOn", OracleDbType.Varchar2, ParameterDirection.Input, query.CreatedOn.Value.ToString("yyyy-MM-dd"));
            sb.Append(CreatedOnCondition);
        }

        sb.Append(Environment.NewLine);
        sb.Append(MaxReceivedRows);

        var finalSql = sb.ToString();

        return finalSql;
    }
}
