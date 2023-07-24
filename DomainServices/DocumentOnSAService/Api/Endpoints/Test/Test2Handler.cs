using DomainServices.DocumentOnSAService.Contracts.v1;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.Test;

public class Test2MediatrRequest : IRequest<Test2Response> { public long Id; }

public class Test2Handler : IRequestHandler<Test2MediatrRequest, Test2Response>
{
    public async Task<Test2Response> Handle(Test2MediatrRequest request, CancellationToken cancellationToken)
    {
        var bytes = System.IO.File.ReadAllBytes("d:/f1002.pdf");
        var result = await _eSignaturesClient.UploadDocument(request.Id, "soubor_1.pdf", DateTime.Now, bytes, cancellationToken);

        return new Test2Response
        {
            Id = result.ExternalId
        };
    }

    private readonly ExternalServices.ESignatures.V1.IESignaturesClient _eSignaturesClient;

    public Test2Handler(ExternalServices.ESignatures.V1.IESignaturesClient eSignaturesClient)
    {
        _eSignaturesClient = eSignaturesClient;
    }
}
