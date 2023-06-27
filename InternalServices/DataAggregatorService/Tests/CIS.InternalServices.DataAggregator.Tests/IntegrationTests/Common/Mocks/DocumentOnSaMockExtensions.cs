using CIS.Testing.Common;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.Common.Mocks;

public static class DocumentOnSaMockExtensions
{
    public static void MockDocumentOnSa(this IDocumentOnSAServiceClient documentOnSAServiceClient)
    {
        var fixture = FixtureFactory.Create();

        var documentOnSa4 = fixture.Build<DocumentOnSAToSign>().With(d => d.DocumentTypeId, 4).With(d => d.IsFinal, true).Create();
        var documentOnSa5 = fixture.Build<DocumentOnSAToSign>().With(d => d.DocumentTypeId, 5).With(d => d.IsFinal, true).Create();

        var response = new GetDocumentsOnSAListResponse { DocumentsOnSA = { new[] { documentOnSa4, documentOnSa5 } } };

        documentOnSAServiceClient.GetDocumentsOnSAList(Arg.Any<int>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(response);
    }
}