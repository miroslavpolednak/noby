using CIS.Core.Attributes;
using SharedTypes.Enums;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using FastEnumUtility;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Common;

public interface ISalesArrangementStateManager
{
    Task SetSalesArrangementStateAccordingDocumentsOnSa(int salesArrangementId, CancellationToken cancellationToken);
}

[ScopedService, AsImplementedInterfacesService]
public class SalesArrangementStateManager : ISalesArrangementStateManager
{
    private readonly ISalesArrangementServiceClient _salesArrangementServiceClient;
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IMediator _mediator;

    public SalesArrangementStateManager(
        ISalesArrangementServiceClient salesArrangementServiceClient,
        DocumentOnSAServiceDbContext dbContext,
        IMediator mediator)
    {
        _salesArrangementServiceClient = salesArrangementServiceClient;
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task SetSalesArrangementStateAccordingDocumentsOnSa(int salesArrangementId, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementServiceClient.GetSalesArrangement(salesArrangementId, cancellationToken);

        var documentsToSign = await _mediator.Send(new GetDocumentsToSignListRequest { SalesArrangementId = salesArrangementId }, cancellationToken);

        var documentOnSaIds = documentsToSign.DocumentsOnSAToSign.Where(w => w.DocumentOnSAId != null)
                                                .Select(d => d.DocumentOnSAId);

        var archivedDocumentOnSaIds = await _dbContext.EArchivIdsLinked
                                          .Where(e => documentOnSaIds.Contains(e.DocumentOnSAId))
                                          .Select(s => s.DocumentOnSAId)
                                          .ToListAsync(cancellationToken);

        if (!_disallowedSaTypesForStateChange.Contains(salesArrangement.SalesArrangementTypeId) 
            && documentsToSign.DocumentsOnSAToSign.All(d =>
                                                    d.DocumentOnSAId is not null
                                                    && d.IsSigned
                                                    && (
                                                        (d.SignatureTypeId == SignatureTypes.Paper.ToByte() && (archivedDocumentOnSaIds.Any(archivedDocumentOnSaId => archivedDocumentOnSaId == d.DocumentOnSAId) || d.DocumentTypeId == DocumentTypes.ZADOCERP.ToByte())) // DocumentTypes.ZADOCERP 6
                                                         || (d.SignatureTypeId == SignatureTypes.Electronic.ToByte())
                                                       )
        )
          )
        {
            await _salesArrangementServiceClient.UpdateSalesArrangementState(salesArrangementId, EnumSalesArrangementStates.ToSend, cancellationToken); // 8
        }
        else if (documentsToSign.DocumentsOnSAToSign.All(d => d.DocumentOnSAId is null))
        {
            await _salesArrangementServiceClient.UpdateSalesArrangementState(salesArrangementId, EnumSalesArrangementStates.InProgress, cancellationToken); // 1
        }
        else
        {
            await _salesArrangementServiceClient.UpdateSalesArrangementState(salesArrangementId, EnumSalesArrangementStates.InSigning, cancellationToken); // 7
        }
    }

    private static readonly int[] _disallowedSaTypesForStateChange =
    [
        (int)SalesArrangementTypes.MortgageRefixation,
        (int)SalesArrangementTypes.MortgageRetention,
        (int)SalesArrangementTypes.MortgageExtraPayment
    ];
}
