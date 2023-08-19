using Google.Protobuf.WellKnownTypes;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.Test;

public class Test5MediatrRequest : IRequest<Empty> { public string Id; }

public class Test5Handler : IRequestHandler<Test5MediatrRequest, Empty>
{
    public async Task<Empty> Handle(Test5MediatrRequest request, CancellationToken cancellationToken)
    {
        await _eSignaturesClient.DeleteDocument(request.Id, cancellationToken);
        return new Empty();
    }

    private readonly ExternalServices.ESignatures.V1.IESignaturesClient _eSignaturesClient;

    public Test5Handler(ExternalServices.ESignatures.V1.IESignaturesClient eSignaturesClient)
    {
        _eSignaturesClient = eSignaturesClient;
    }
}
