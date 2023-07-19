using Google.Protobuf.WellKnownTypes;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.Test;

public class Test6MediatrRequest : IRequest<Empty> { public string Id; }

public class Test6Handler : IRequestHandler<Test6MediatrRequest, Empty>
{
    public async Task<Empty> Handle(Test6MediatrRequest request, CancellationToken cancellationToken)
    {
        await _eSignaturesClient.DeleteDocument(request.Id, cancellationToken);
        return new Empty();
    }

    private readonly ExternalServices.ESignatures.V1.IESignaturesClient _eSignaturesClient;

    public Test6Handler(ExternalServices.ESignatures.V1.IESignaturesClient eSignaturesClient)
    {
        _eSignaturesClient = eSignaturesClient;
    }
}
