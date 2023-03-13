using CIS.Core;
using CIS.Core.Security;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using ExternalServices.Eas.V1;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.SignDocumentManually;

public sealed class SignDocumentManuallyHandler : IRequestHandler<SignDocumentManuallyRequest, Empty>
{
    private const string ManualSigningMethodCode = "PHYSICAL";
    /// <summary>
    /// Form 3601
    /// </summary>
    private const int DocumentType = 4;

    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly ISalesArrangementServiceClient _arrangementServiceClient;
    private readonly IEasClient _easClient;

    public SignDocumentManuallyHandler(
        DocumentOnSAServiceDbContext dbContext,
        IDateTime dateTime,
        ICurrentUserAccessor currentUser,
        ISalesArrangementServiceClient arrangementServiceClient,
        IEasClient easClient)
    {
        _dbContext = dbContext;
        _dateTime = dateTime;
        _currentUser = currentUser;
        _arrangementServiceClient = arrangementServiceClient;
        _easClient = easClient;
    }

    public async Task<Empty> Handle(SignDocumentManuallyRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa.FirstOrDefaultAsync(r => r.DocumentOnSAId == request.DocumentOnSAId!.Value, cancellationToken);

        if (documentOnSa is null)
        {
            throw new CisNotFoundException(19003, $"DocumentOnSA {request.DocumentOnSAId!.Value} does not exist.");
        }

        if (documentOnSa.SignatureMethodCode.ToUpper() != ManualSigningMethodCode || documentOnSa.IsSigned)
        {
            throw new CisValidationException(19005, $"Unable to sign DocumentOnSA {request.DocumentOnSAId!.Value}. Document is for electronic signature only or is already signed.");
        }

        UpdateDocumentOnSa(documentOnSa);

        await AddSignatureIfNotSetYet(documentOnSa, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }

    private async Task AddSignatureIfNotSetYet(DocumentOnSa documentOnSa, CancellationToken cancellationToken)
    {
        if (documentOnSa.DocumentTypeId == DocumentType
            && await _dbContext.DocumentOnSa.Where(d => d.SalesArrangementId == documentOnSa.SalesArrangementId).AllAsync(r => r.IsSigned == false))
        {
            var salesArrangement = await _arrangementServiceClient.GetSalesArrangement(documentOnSa.SalesArrangementId, cancellationToken);

            if (salesArrangement is null)
            {
                throw new CisNotFoundException(19000, $"SalesArrangement{documentOnSa.SalesArrangementId} does not exist.");
            }

            var result = await _easClient.AddFirstSignatureDate((int)salesArrangement.CaseId, _dateTime.Now.Date, cancellationToken);

            if (result is not null && result.CommonValue != 0)
            {
                throw new CisValidationException(19006, $"Eas {nameof(IEasClient.AddFirstSignatureDate)} returned error state: {result.CommonValue} with message: {result.CommonText}");
            }
        }
    }

    private void UpdateDocumentOnSa(DocumentOnSa documentOnSa)
    {
        documentOnSa.IsSigned = true;
        documentOnSa.SignatureDateTime = _dateTime.Now;
        documentOnSa.SignatureConfirmedBy = _currentUser.User?.Id;
    }
}
