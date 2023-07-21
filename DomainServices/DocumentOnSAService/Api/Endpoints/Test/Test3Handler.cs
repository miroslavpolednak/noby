using DomainServices.DocumentOnSAService.Contracts.v1;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.Test;

public class Test3MediatrRequest : IRequest<Test3Response> { public string Id; }

public class Test3Handler : IRequestHandler<Test3MediatrRequest, Test3Response>
{
    public async Task<Test3Response> Handle(Test3MediatrRequest request, CancellationToken cancellationToken)
    {
        var result = await _eSignaturesClient.GetDocumentStatus(request.Id, cancellationToken);
        return new Test3Response { Status = result.ToString() };
    }

    private readonly ExternalServices.ESignatures.V1.IESignaturesClient _eSignaturesClient;

    public Test3Handler(ExternalServices.ESignatures.V1.IESignaturesClient eSignaturesClient)
    {
        _eSignaturesClient = eSignaturesClient;
    }
}
