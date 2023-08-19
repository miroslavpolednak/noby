using DomainServices.DocumentOnSAService.Contracts.v1;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.Test;

public class Test4MediatrRequest : IRequest<Test4Response> { public string Id; }

public class Test4Handler : IRequestHandler<Test4MediatrRequest, Test4Response>
{
    public async Task<Test4Response> Handle(Test4MediatrRequest request, CancellationToken cancellationToken)
    {
        var result = await _eSignaturesClient.DownloadDocumentPreview(request.Id, cancellationToken);
        return new Test4Response { Length = result.Length };
    }

    private readonly ExternalServices.ESignatures.V1.IESignaturesClient _eSignaturesClient;

    public Test4Handler(ExternalServices.ESignatures.V1.IESignaturesClient eSignaturesClient)
    {
        _eSignaturesClient = eSignaturesClient;
    }
}
