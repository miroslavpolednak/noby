using CIS.Foms.Enums;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using ExternalServices.ESignatures.V1;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.SetDocumentOnSAArchived;

public class SetDocumentOnSAArchivedHandler : IRequestHandler<SetDocumentOnSAArchivedRequest, Empty>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IESignaturesClient _eSignaturesClient;

    public SetDocumentOnSAArchivedHandler(
        DocumentOnSAServiceDbContext dbContext,
        IESignaturesClient eSignaturesClient)
    {
        _dbContext = dbContext;
        _eSignaturesClient = eSignaturesClient;
    }

    public async Task<Empty> Handle(SetDocumentOnSAArchivedRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa.Where(r => r.DocumentOnSAId == request.DocumentOnSAId)
                                               .FirstOrDefaultAsync(cancellationToken)
                                               ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist, request.DocumentOnSAId);

        if (documentOnSa.SignatureTypeId is not null && documentOnSa.SignatureTypeId == (int)SignatureTypes.Electronic)
        {

            await _eSignaturesClient.SubmitDispatchForm(true, new() { new()
            {
                ExternalId = documentOnSa.ExternalId!,
                IsCancelled = false,
                AttachmentsComplete = true,
            }}, cancellationToken);
        }

        documentOnSa.IsArchived = true;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new Empty();
    }
}
