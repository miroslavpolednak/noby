﻿using DomainServices.DocumentArchiveService.Contracts;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1.Model;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Model;

namespace DomainServices.DocumentArchiveService.Api.Mappers;

public class DocumentMapper : IDocumentMapper
{
    public DocumentMetadata MapSdfDocumentMetadata(MetadataValue[] values)
    {
        var metadata = new DocumentMetadata();
        var caseId = values.FirstOrDefault(r => r.AttributeName == "OP_Cislo_pripadu")?.Value;
        metadata.CaseId = long.TryParse(caseId, out var caseIdOut) ? caseIdOut : null;
        metadata.DocumentId = values.FirstOrDefault(r => r.AttributeName == "DOK_ID_dokumentu_zdrojoveho_systemu")?.Value ?? string.Empty;
        metadata.EaCodeMainId = int.TryParse(values.FirstOrDefault(r => r.AttributeName == "DOK_Heslo_hlavni_ID")?.Value, out var outEaCodeMainId) ? outEaCodeMainId : null;
        metadata.Filename = values.FirstOrDefault(r => r.AttributeName == "DOK_Nazev_souboru")?.Value ?? string.Empty;
        metadata.Description = values.FirstOrDefault(r => r.AttributeName == "DOK_Popis")?.Value ?? string.Empty;
        metadata.OrderId = int.TryParse(values.FirstOrDefault(r => r.AttributeName == "DOK_ID_oceneni")?.Value, out var outOrderId) ? outOrderId : null;
        metadata.CreatedOn = DateTime.TryParse(values.FirstOrDefault(r => r.AttributeName == "DOK_Datum_prijeti")?.Value, out var outCreatedOn) ? outCreatedOn : default;
        metadata.AuthorUserLogin = values.FirstOrDefault(r => r.AttributeName == "DOK_Autor")?.Value ?? string.Empty;
        metadata.Priority = values.FirstOrDefault(r => r.AttributeName == "DOK_Priorita")?.Value ?? string.Empty;
        metadata.Status = values.FirstOrDefault(r => r.AttributeName == "DOK_Status")?.Value ?? string.Empty;
        metadata.FolderDocument = values.FirstOrDefault(r => r.AttributeName == "DOK_NadrizenostPodrizenost")?.Value ?? string.Empty;
        metadata.FolderDocumentId = values.FirstOrDefault(r => r.AttributeName == "DOK_Vazba_pro_SP")?.Value ?? string.Empty;
        metadata.DocumentDirection = values.FirstOrDefault(r => r.AttributeName == "DOK_Smer_dokumentu")?.Value ?? string.Empty;
        metadata.SourceSystem = values.FirstOrDefault(r => r.AttributeName == "DOK_Zdroj")?.Value ?? string.Empty;
        metadata.FormId = values.FirstOrDefault(r => r.AttributeName == "DOK_ID_formulare")?.Value ?? string.Empty;
        metadata.ContractNumber = values.FirstOrDefault(r => r.AttributeName == "OP_Cislo_smlouvy")?.Value ?? string.Empty;
        metadata.PledgeAgreementNumber = values.FirstOrDefault(r => r.AttributeName == "DOK_Cislo_zastavni_smlouvy")?.Value ?? string.Empty;
        return metadata;
    }

    public FindSdfDocumentsQuery MapSdfFindDocumentQuery(GetDocumentListRequest request)
    {
        return new FindSdfDocumentsQuery
        {
            CaseId = request.CaseId,
            AuthorUserLogin = request.AuthorUserLogin,
            CreatedOn = request.CreatedOn is not null ? request.CreatedOn : (DateOnly?)null,
            PledgeAgreementNumber = request.PledgeAgreementNumber,
            ContractNumber = request.ContractNumber,
            OrderId = request.OrderId,
            FolderDocumentId = request.FolderDocumentId,
            UserLogin = request.UserLogin
        };
    }

    public DocumentMetadata MapTcpDocumentMetadata(DocumentServiceQueryResult result)
    {
        var metadata = new DocumentMetadata();
        metadata.CaseId = long.TryParse(result.CaseId, out var caseId) ? caseId : null;
        metadata.DocumentId = result.DocumentId;
        metadata.EaCodeMainId = result.EaCodeMainId;
        metadata.Filename = result.Filename ?? string.Empty;
        metadata.DocumentId = result.DocumentId ?? string.Empty;
        metadata.EaCodeMainId = result.EaCodeMainId;
        metadata.Description = result.Filename ?? string.Empty; // This weird initialization is based on requirement from StarBuild. 
        metadata.OrderId = int.TryParse(result.OrderId, out var orderId) ? orderId : null;
        metadata.CreatedOn = result.CreatedOn;
        metadata.AuthorUserLogin = result.AuthorUserLogin ?? string.Empty;
        metadata.Priority = result.Priority;
        metadata.Status = result.Status;
        metadata.FolderDocument = result.FolderDocument ?? string.Empty;
        metadata.FolderDocumentId = result.FolderDocumentId ?? string.Empty;
        metadata.DocumentDirection = result.DocumentDirection ?? string.Empty;
        metadata.SourceSystem = string.Empty; // This value isn´t in TCP archive
        metadata.FormId = result.FormId ?? string.Empty;
        metadata.ContractNumber = result.ContractNumber ?? string.Empty;
        metadata.PledgeAgreementNumber = result.PledgeAgreementNumber ?? string.Empty;
        metadata.Completeness = result.Completeness;
        metadata.MinorCodes.AddRange(GetMinorCodes(result.MinorCodes));
        return metadata;
    }

    public FindTcpDocumentQuery MapTcpDocumentQuery(GetDocumentListRequest request)
    {
        return new FindTcpDocumentQuery
        {
            CaseId = request.CaseId,
            AuthorUserLogin = request.AuthorUserLogin,
            CreatedOn = request.CreatedOn is not null ? request.CreatedOn : (DateOnly?)null,
            PledgeAgreementNumber = request.PledgeAgreementNumber,
            ContractNumber = request.ContractNumber,
            OrderId = request.OrderId,
            FolderDocumentId = request.FolderDocumentId,
        };
    }

    private static IEnumerable<int> GetMinorCodes(string minorCodes)
    {
        if (string.IsNullOrWhiteSpace(minorCodes))
        {
            return Enumerable.Empty<int>();
        }

        return minorCodes.Split(',')
            .Select(s => { int i; return int.TryParse(s, out i) ? i : (int?)null; })
            .Where(i => i.HasValue)
            .Select(i => i!.Value);
    }
}

