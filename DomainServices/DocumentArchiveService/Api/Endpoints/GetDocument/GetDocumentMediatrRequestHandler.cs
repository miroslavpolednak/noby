using DomainServices.DocumentArchiveService.Contracts;
using ExternalServices.Sdf.V1.Clients;
using ExternalServices.Sdf.V1.Model;
using ExternalServicesTcp.Model;
using ExternalServicesTcp.V1.Clients;
using ExternalServicesTcp.V1.Repositories;
using Google.Protobuf;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GetDocument
{
    public class GetDocumentMediatrRequestHandler : IRequestHandler<GetDocumentMediatrRequest, GetDocumentResponse>
    {
        private const string DocumentPrefix = "KBH";
        private readonly ISdfClient _sdfClient;
        private readonly IDocumentServiceRepository _documentServiceRepository;
        private readonly ITcpClient _tcpClient;

        public GetDocumentMediatrRequestHandler(
            ISdfClient sdfClient,
            IDocumentServiceRepository documentServiceRepository,
            ITcpClient tcpClient)
        {
            _sdfClient = sdfClient;
            _documentServiceRepository = documentServiceRepository;
            _tcpClient = tcpClient;
        }

        public async Task<GetDocumentResponse> Handle(GetDocumentMediatrRequest request, CancellationToken cancellationToken)
        {
            if (request.Request.DocumentId.StartsWith(DocumentPrefix))
            {
                var cspResponse = await LoadFromCspArchive(request, cancellationToken);
                return MapCspResponse(cspResponse);
            }
            else
            {
                var tcpResult = await LoadFromTcpArchive(request, cancellationToken);
                return await MapTcpResponse(tcpResult, request.Request, cancellationToken);
            }

        }

        private async Task<GetDocumentResponse> MapTcpResponse(DocumentServiceQueryResult tcpResult, GetDocumentRequest request, CancellationToken cancellationToken)
        {
            if (tcpResult is null)
            {
                throw new ArgumentNullException(nameof(tcpResult));
            }

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = new GetDocumentResponse
            {
                Metadata = new DocumentMetadata(),
                Content = new Contracts.FileInfo()
            };

            response.Metadata.CaseId = long.TryParse(tcpResult.CaseId, out var caseId) ? caseId : null;
            response.Metadata.DocumentId = tcpResult.DocumentId;
            response.Metadata.EaCodeMainId = tcpResult.EaCodeMainId;
            response.Metadata.Filename = tcpResult.Filename ?? string.Empty;
            response.Metadata.DocumentId = tcpResult.DocumentId ?? string.Empty;
            response.Metadata.EaCodeMainId = tcpResult.EaCodeMainId;
            response.Metadata.Filename = tcpResult.Filename ?? string.Empty;
            response.Metadata.Description = string.Empty; // This value isn´t in TCP archive
            response.Metadata.OrderId = int.TryParse(tcpResult.OrderId, out var orderId) ? orderId : null;
            response.Metadata.CreatedOn = tcpResult.CreatedOn;
            response.Metadata.AuthorUserLogin = tcpResult.AuthorUserLogin ?? string.Empty;
            response.Metadata.Priority = tcpResult.Priority;
            response.Metadata.Status = tcpResult.Status;
            response.Metadata.FolderDocument = tcpResult.FolderDocument ?? string.Empty;
            response.Metadata.FolderDocumentId = tcpResult.FolderDocumentId ?? string.Empty;
            response.Metadata.DocumentDirection = tcpResult.DocumentDirection ?? string.Empty;
            response.Metadata.SourceSystem = string.Empty; // This value isn´t in TCP archive
            response.Metadata.FormId = tcpResult.FormId ?? string.Empty;
            response.Metadata.ContractNumber = tcpResult.ContractNumber ?? string.Empty;
            response.Metadata.PledgeAgreementNumber = tcpResult.PledgeAgreementNumber ?? string.Empty;
            response.Metadata.Completeness = tcpResult.Completeness;
            response.Metadata.MinorCodes.AddRange(GetMinorCodes(tcpResult.MinorCodes));
            if (request.WithContent)
            {
                response.Content.BinaryData = ByteString.CopyFrom(await _tcpClient.DownloadFile(tcpResult.Url, cancellationToken));
            }
            response.Content.MineType = tcpResult.MimeType;
            return response;
        }

        private IEnumerable<int> GetMinorCodes(string minorCodes)
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

        private GetDocumentResponse MapCspResponse(GetDocumentByExternalIdOutput cspResponse)
        {
            if (cspResponse.Metadata is null)
            {
                throw new ArgumentNullException(nameof(cspResponse.Metadata));
            }

            var response = new GetDocumentResponse
            {
                Metadata = new DocumentMetadata(),
                Content = new Contracts.FileInfo()
            };

            var caseId = cspResponse.Metadata.First(r => r.AttributeName == "OP_Cislo_pripadu").Value;
            response.Metadata.CaseId = long.TryParse(caseId, out var caseIdOut) ? caseIdOut : null;
            response.Metadata.DocumentId = cspResponse.Metadata.First(r => r.AttributeName == "DOK_ID_dokumentu_zdrojoveho_systemu").Value;
            response.Metadata.EaCodeMainId = int.Parse(cspResponse.Metadata.First(r => r.AttributeName == "DOK_Heslo_hlavni_ID").Value);
            response.Metadata.Filename = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Nazev_souboru").Value ?? string.Empty;
            response.Metadata.Description = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Popis").Value ?? string.Empty;
            response.Metadata.OrderId = int.Parse(cspResponse.Metadata.First(r => r.AttributeName == "DOK_ID_oceneni").Value ?? "0");
            response.Metadata.CreatedOn = DateTime.Parse(cspResponse.Metadata.First(r => r.AttributeName == "DOK_Datum_prijeti").Value);
            response.Metadata.AuthorUserLogin = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Autor").Value ?? string.Empty;
            response.Metadata.Priority = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Priorita").Value ?? string.Empty;
            response.Metadata.Status = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Status").Value ?? string.Empty; ;
            response.Metadata.FolderDocument = cspResponse.Metadata.First(r => r.AttributeName == "DOK_NadrizenostPodrizenost").Value ?? string.Empty; ;
            response.Metadata.FolderDocumentId = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Vazba_pro_SP").Value ?? string.Empty;
            response.Metadata.DocumentDirection = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Smer_dokumentu").Value ?? string.Empty;
            response.Metadata.SourceSystem = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Zdroj").Value ?? string.Empty;
            response.Metadata.FormId = cspResponse.Metadata.First(r => r.AttributeName == "DOK_ID_formulare").Value ?? string.Empty;
            response.Metadata.ContractNumber = cspResponse.Metadata.First(r => r.AttributeName == "OP_Cislo_smlouvy").Value ?? string.Empty;
            response.Metadata.PledgeAgreementNumber = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Cislo_zastavni_smlouvy").Value ?? string.Empty;
            response.Content.BinaryData = ByteString.CopyFrom(cspResponse.FileContent);
            response.Content.MineType = cspResponse.DmsDocInfo.MimeType;
            return response;
        }

        private async Task<DocumentServiceQueryResult> LoadFromTcpArchive(GetDocumentMediatrRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await _documentServiceRepository.GetDocumentByExternalId(new ExternalServicesTcp.Model.GetDocumentByExternalIdTcpQuery
            {
                DocumentId = request.Request.DocumentId,
                WithContent = request.Request.WithContent
            },
            cancellationToken);
        }

        private async Task<GetDocumentByExternalIdOutput> LoadFromCspArchive(GetDocumentMediatrRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await _sdfClient
           .GetDocumentByExternalId(new GetDocumentByExternalIdSdfQuery
           {
               DocumentId = request.Request.DocumentId,
               WithContent = request.Request.WithContent
           },
           cancellationToken);
        }
    }
}
