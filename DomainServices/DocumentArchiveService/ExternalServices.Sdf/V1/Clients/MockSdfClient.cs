using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1.Model;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;
using System.Globalization;

namespace DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1.Clients;
public class MockSdfClient : ISdfClient
{

    List<MetadataValue> mockValues = new List<MetadataValue> {
          new MetadataValue
          {
              AttributeName= "OP_Cislo_pripadu",
              Value ="132456"
          },
          new MetadataValue
          {
             AttributeName= "DOK_ID_dokumentu_zdrojoveho_systemu",
             Value ="TestDocId"
          },
          new MetadataValue
          {
             AttributeName= "DOK_Heslo_hlavni_ID",
             Value ="603225"
          },
          new MetadataValue
          {
             AttributeName= "DOK_Nazev_souboru",
             Value ="TestFilename.txt"
          },
          new MetadataValue
          {
             AttributeName= "DOK_Popis",
             Value ="TestDescription"
          },
          new MetadataValue
          {
             AttributeName= "DOK_ID_oceneni",
             Value ="11"
          },
          new MetadataValue
          {
             AttributeName= "DOK_Datum_prijeti",
             Value = new DateTime(2000,1,1).ToString(CultureInfo.InvariantCulture)
          },
          new MetadataValue
          {
             AttributeName= "DOK_Autor",
             Value ="TestAuthor"
          },
          new MetadataValue
          {
             AttributeName= "DOK_Priorita",
             Value ="TestPriority"
          },
          new MetadataValue
          {
             AttributeName= "DOK_Status",
             Value ="TestStatus"
          },
          new MetadataValue
          {
             AttributeName= "DOK_NadrizenostPodrizenost",
             Value ="TestFolderDocument"
          },
          new MetadataValue
          {
             AttributeName= "DOK_Vazba_pro_SP",
             Value ="TestFolderDocumentId"
          },
          new MetadataValue
          {
             AttributeName= "DOK_Smer_dokumentu",
             Value ="TestDocumentDirection"
          },
          new MetadataValue
          {
             AttributeName= "DOK_Zdroj",
             Value ="TestSourceSystem"
          },
          new MetadataValue
          {
             AttributeName= "DOK_ID_formulare",
             Value ="TestFormId"
          },
          new MetadataValue
          {
             AttributeName= "OP_Cislo_smlouvy",
             Value ="TestContractNumber"
          },
          new MetadataValue
          {
             AttributeName= "DOK_Cislo_zastavni_smlouvy",
             Value ="TestPledgeAgreementNumber"
          },
        };


    public Task<FindDocumentsOutput> FindDocuments(FindSdfDocumentsQuery query, CancellationToken cancellationToken)
    {
        var data = new FindDocumentsOutput
        {
            DocInfos = new DocumentInfo[] { new DocumentInfo { Metadata = new MetadataValueList() } }
        };

        data.DocInfos.First().Metadata.AddRange(mockValues.ToArray());
        return Task.FromResult(data);
    }

    public Task<GetDocumentByExternalIdOutput> GetDocumentByExternalId(GetDocumentByExternalIdSdfQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(new GetDocumentByExternalIdOutput
        {
            Metadata = mockValues.ToArray(),
            FileContent = query.WithContent ? Convert.FromBase64String("VGhpcyBpcyBhIHRlc3Q="): null,
            DmsDocInfo = new DmsDocumentInfo { MimeType = "text/plain" }
        });
    }
}
