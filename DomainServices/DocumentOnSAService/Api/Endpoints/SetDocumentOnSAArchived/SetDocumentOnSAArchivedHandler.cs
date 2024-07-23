using SharedTypes.Enums;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using ExternalServices.ESignatures.V1;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using DomainServices.CodebookService.Clients;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.SetDocumentOnSAArchived;

public class SetDocumentOnSAArchivedHandler : IRequestHandler<SetDocumentOnSAArchivedRequest, Empty>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IESignaturesClient _eSignaturesClient;
    private readonly ICodebookServiceClient _codebookService;

    public SetDocumentOnSAArchivedHandler(
        DocumentOnSAServiceDbContext dbContext,
        IESignaturesClient eSignaturesClient,
        ICodebookServiceClient codebookService)
    {
        _dbContext = dbContext;
        _eSignaturesClient = eSignaturesClient;
        _codebookService = codebookService;
    }

    public async Task<Empty> Handle(SetDocumentOnSAArchivedRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa.Where(r => r.DocumentOnSAId == request.DocumentOnSAId)
                                               .FirstOrDefaultAsync(cancellationToken)
                                               ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist, request.DocumentOnSAId);

        var eaCodeMainId = (await _codebookService.DocumentTypes(cancellationToken))
                           .Find(d => d.Id == documentOnSa.DocumentTypeId)?.EACodeMainId;

        if (documentOnSa.SignatureTypeId is not null && documentOnSa.SignatureTypeId == (int)SignatureTypes.Electronic)
        {
            await _eSignaturesClient.SubmitDispatchForm(true, [new()
            {
                ExternalId = documentOnSa.ExternalIdESignatures!,
                IsCancelled = !documentOnSa.IsValid && documentOnSa.SignatureTypeId == (int)SignatureTypes.Electronic,
                AttachmentsComplete = true,
                EaCodeMainId = eaCodeMainId,
            }], cancellationToken);
        }

        documentOnSa.IsArchived = true;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new Empty();
    }
}
