﻿using CIS.Foms.Types;
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
                DocumentTypeId = 1,
                FileName = "soubor_1.pdf",
                FormId = "999",
                ContractNumber = "1234567890"
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