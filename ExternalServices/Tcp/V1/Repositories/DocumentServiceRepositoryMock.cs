using ExternalServicesTcp.V1.Model;
using static System.Net.Mime.MediaTypeNames;

namespace ExternalServicesTcp.V1.Repositories;
internal class DocumentServiceRepositoryMock : IDocumentServiceRepository
{
    public async Task<IReadOnlyCollection<DocumentServiceQueryResult>> FindTcpDocument(FindTcpDocumentQuery query, CancellationToken cancellationToken)
    {
        return new List<DocumentServiceQueryResult>
        {
            new DocumentServiceQueryResult
            {
                CaseId = "1",
                DocumentId = "TestID",
                EaCodeMainId = 3,
                Filename = "test.txt",
                OrderId = "1",
                CreatedOn = DateTime.Now,
                AuthorUserLogin = "TestLogin",
                Priority = "1",
                Status = "Z",
                FolderDocument = "S",
                FolderDocumentId = "P",
                DocumentDirection = "E",
                FormId = "FormTest1",
                ContractNumber = "132",
                PledgeAgreementNumber = "11111",
                Completeness = 1,
                MinorCodes = "1",
                MimeType = "text/plain"
            },
            new DocumentServiceQueryResult
            {
                CaseId = "2",
                DocumentId = "TestID2",
                EaCodeMainId = 3,
                Filename = "test2.txt",
                OrderId = "1",
                CreatedOn = DateTime.Now,
                AuthorUserLogin = "TestLogin",
                Priority = "1",
                Status = "Z",
                FolderDocument = "S",
                FolderDocumentId = "P",
                DocumentDirection = "E",
                FormId = "FormTest1",
                ContractNumber = "132",
                PledgeAgreementNumber = "11111",
                Completeness = 1,
                MinorCodes = "1",
                MimeType = "text/plain"
            }
        };
    }

    public Task<DocumentServiceQueryResult> GetDocumentByExternalId(GetDocumentByExternalIdTcpQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(new DocumentServiceQueryResult
        {
            CaseId = "1",
            DocumentId = "TestID",
            EaCodeMainId = 3,
            Filename = "test.txt",
            OrderId = "1",
            CreatedOn = DateTime.Now,
            AuthorUserLogin = "TestLogin",
            Priority = "1",
            Status = "Z",
            FolderDocument = "S",
            FolderDocumentId = "P",
            DocumentDirection = "E",
            FormId = "FormTest1",
            ContractNumber = "132",
            PledgeAgreementNumber = "11111",
            Completeness = 1,
            MinorCodes = "1",
            MimeType = "text/plain"
        });
    }
}
