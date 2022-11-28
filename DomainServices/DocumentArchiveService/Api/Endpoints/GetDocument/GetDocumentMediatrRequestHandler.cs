using DomainServices.DocumentArchiveService.Contracts;
using ExternalServices.Sdf.V1.Clients;
using ExternalServices.Sdf.V1.Model;
using FastEnumUtility;
using Google.Protobuf;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GetDocument
{
    public class GetDocumentMediatrRequestHandler : IRequestHandler<GetDocumentMediatrRequest, GetDocumentResponse>
    {
        private readonly ISdfClient _sdfClient;

        public GetDocumentMediatrRequestHandler(ISdfClient sdfClient)
        {
            _sdfClient = sdfClient;
        }

        public async Task<GetDocumentResponse> Handle(GetDocumentMediatrRequest request, CancellationToken cancellationToken)
        {
            var cspResponse = await _sdfClient
                .GetDocumentByExternalId(new GetDocumentByExternalQuery
                {
                    DocumentId = request.Request.DocumentId,
                    WithContent = request.Request.WithContent,
                },
                cancellationToken);

            return MapCspResponse(cspResponse);
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

            response.Metadata.CaseId = long.Parse(cspResponse.Metadata.First(r => r.AttributeName == "OP_Cislo_pripadu").Value);
            response.Metadata.DocumentId = cspResponse.Metadata.First(r => r.AttributeName == "DOK_ID_dokumentu_zdrojoveho_systemu").Value;
            response.Metadata.EaCodeMainId = int.Parse(cspResponse.Metadata.First(r => r.AttributeName == "DOK_Heslo_hlavni_ID").Value);
            response.Metadata.Filename = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Nazev_souboru").Value;
            response.Metadata.Description = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Popis").Value ?? string.Empty;
            response.Metadata.OrderId = int.Parse(cspResponse.Metadata.First(r => r.AttributeName == "DOK_ID_oceneni").Value ?? "0");
            response.Metadata.CreatedOn = DateTime.Parse(cspResponse.Metadata.First(r => r.AttributeName == "DOK_Datum_prijeti").Value);
            response.Metadata.AuthorUserLogin = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Autor").Value ?? string.Empty;
            response.Metadata.Priority = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Priorita").Value ?? string.Empty;
            response.Metadata.Status = FastEnum.Parse<DocumentStatus>(cspResponse.Metadata.First(r => r.AttributeName == "DOK_Status").Value);
            response.Metadata.FolderDocument = FastEnum.Parse<FolderDocument>(cspResponse.Metadata.First(r => r.AttributeName == "DOK_NadrizenostPodrizenost").Value);
            response.Metadata.FolderDocumentId = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Vazba_pro_SP").Value ?? string.Empty;
            response.Metadata.DocumentDirection = FastEnum
                                                 .Parse<DocumentDirection>(
                                                 GetDocumentDirection(cspResponse.Metadata.First(r => r.AttributeName == "DOK_Smer_dokumentu").Value)
                                                 );
            response.Metadata.SourceSystem = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Zdroj").Value ?? string.Empty;
            response.Metadata.FormId = cspResponse.Metadata.First(r => r.AttributeName == "DOK_ID_formulare").Value ?? string.Empty;
            response.Metadata.ContractNumber = cspResponse.Metadata.First(r => r.AttributeName == "OP_Cislo_smlouvy").Value ?? string.Empty;
            response.Metadata.PledgeAgreementNumber = cspResponse.Metadata.First(r => r.AttributeName == "DOK_Cislo_zastavni_smlouvy").Value ?? string.Empty;

            response.Content.BinaryData = ByteString.CopyFrom(cspResponse.FileContent);
            response.Content.MineType = cspResponse.DmsDocInfo.MimeType;
            return response;
        }

        private string GetDocumentDirection(string direction) => direction.ToLower() switch
        {
            "interní" => "I",
            "výstupní" => "A",
            "vstupní" => "E",
            _ => "E"
        };
    }
}
