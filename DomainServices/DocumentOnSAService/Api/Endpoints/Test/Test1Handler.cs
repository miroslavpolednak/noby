/*
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\DocumentOnSAService\Api\DomainServices.DocumentOnSAService.Api.csproj"

grpcurl -insecure -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/Test1
grpcurl -insecure -d "{\"Id\":3185422}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/Test2
grpcurl -insecure -d "{\"Id\":\"111\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/Test3
grpcurl -insecure -d "{\"Id\":\"SBNU00000000000000000010307019\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/Test4
grpcurl -insecure -d "{\"Id\":\"SBNU00000000000000000010311778\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/Test5
*/

using CIS.Foms.Types;
using DomainServices.DocumentOnSAService.Contracts.v1;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.Test;

public class Test1MediatrRequest : IRequest<Test1Response> { }

public class Test1Handler : IRequestHandler<Test1MediatrRequest, Test1Response>
{
    public async Task<Test1Response> Handle(Test1MediatrRequest request, CancellationToken cancellationToken)
    {
        ExternalServices.ESignatures.Dto.PrepareDocumentRequest req = new()
        {
            CurrentUserInfo = new()
            {
                Cpm = "49410000",
                Icp = "494100028",
                FullName = "John Doe",
            },
            CreatorInfo = new()
            {
                Cpm = "49410000",
                Icp = "494100028",
                FullName = "John Doe",
            },
            DocumentData = new ExternalServices.ESignatures.Dto.PrepareDocumentRequest.DocumentInfo()
            {
                DocumentTemplateVersionId = 4,
                DocumentTypeId = 4,
                FileName = "soubor_1.pdf",
                FormId = "999",
                ContractNumber = "HF00000003190"
            },
            ClientData = new ExternalServices.ESignatures.Dto.PrepareDocumentRequest.ClientInfo
            {
                Identities = new List<CustomerIdentity>
                {
                    new() { Scheme = CIS.Foms.Enums.IdentitySchemes.Kb, Id = 951061749 },
                    new() { Scheme = CIS.Foms.Enums.IdentitySchemes.Mp, Id = 300526731 }
                },
                FullName = "John Doe",
                BirthNumber = "500101123",
                Phone = "123456789",
                Email = "aaa@aaa.cz"
            }
        };

        var result = await _eSignaturesClient.PrepareDocument(req, cancellationToken);

        return new Test1Response
        {
            Id = result
        };
    }

    private readonly ExternalServices.ESignatures.V1.IESignaturesClient _eSignaturesClient;

    public Test1Handler(ExternalServices.ESignatures.V1.IESignaturesClient eSignaturesClient)
    {
        _eSignaturesClient = eSignaturesClient;
    }
}
