using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class DocumentOnSaServiceWrapper : IServiceWrapper
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly ICodebookServiceClients _codebookService;

    public DocumentOnSaServiceWrapper(IDocumentOnSAServiceClient documentOnSAService, ICodebookServiceClients codebookService)
    {
        _documentOnSAService = documentOnSAService;
        _codebookService = codebookService;
    }

    public DataSource DataSource => DataSource.DocumentOnSa;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateSalesArrangementId();

        var response = await _documentOnSAService.GetDocumentsOnSAList(input.SalesArrangementId!.Value, cancellationToken);

        data.Custom.DocumentOnSa = new DocumentOnSaInfo(response.DocumentsOnSA)
        {
            SignatureMethodId = await GetSignatureMethodId(response.DocumentsOnSA, cancellationToken)
        };
    }

    private async Task<int> GetSignatureMethodId(IEnumerable<DocumentOnSAToSign> documentsOnSa, CancellationToken cancellationToken)
    {
        var signingMethods = await _codebookService.SigningMethodsForNaturalPerson(cancellationToken);

        var signatureMethodCode = documentsOnSa.LastOrDefault(d => d.IsValid && d.IsSigned)?.SignatureMethodCode;

        return signingMethods.Where(s => s.Code == signatureMethodCode).Select(s => s.StarbuildEnumId).FirstOrDefault(1);
    }
}