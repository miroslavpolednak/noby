using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;
using Microsoft.EntityFrameworkCore;
using SharedComponents.DocumentDataStorage;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetSalesArrangement;

internal sealed class GetSalesArrangementHandler
    : IRequestHandler<__SA.GetSalesArrangementRequest, __SA.SalesArrangement>
{
    public async Task<__SA.SalesArrangement> Handle(__SA.GetSalesArrangementRequest request, CancellationToken cancellation)
    {
        // detail SA
        var model = await _dbContext.SalesArrangements
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .AsNoTracking()
            .Select(DatabaseExpressions.SalesArrangementDetail())
            .FirstOrDefaultAsync(cancellation) 
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

        // parametry
        switch ((SalesArrangementTypes)model.SalesArrangementTypeId)
        {
            case SalesArrangementTypes.Mortgage:
                var mortgageDoc = await _documentDataStorage.FirstOrDefaultByEntityId<MortgageData>(model.SalesArrangementId, cancellation);
                model.Mortgage = mortgageDoc?.Data?.MapMortgage();
                break;

            case SalesArrangementTypes.Drawing:
                var drawingDoc = await _documentDataStorage.FirstOrDefaultByEntityId<DrawingData>(model.SalesArrangementId, cancellation);
                model.Drawing = drawingDoc?.Data?.MapDrawing();
                break;

            case SalesArrangementTypes.GeneralChange:
                var generalChangeDoc = await _documentDataStorage.FirstOrDefaultByEntityId<GeneralChangeData>(model.SalesArrangementId, cancellation);
                model.GeneralChange = generalChangeDoc?.Data?.MapGeneralChange();
                break;

            case SalesArrangementTypes.HUBN:
                var hubnDoc = await _documentDataStorage.FirstOrDefaultByEntityId<HUBNData>(model.SalesArrangementId, cancellation);
                model.HUBN = hubnDoc?.Data?.MapHUBN();
                break;

            case SalesArrangementTypes.CustomerChange:
                var customerChangeDoc = await _documentDataStorage.FirstOrDefaultByEntityId<CustomerChangeData>(model.SalesArrangementId, cancellation);
                model.CustomerChange = customerChangeDoc?.Data?.MapCustomerChange();
                break;

            case SalesArrangementTypes.CustomerChange3602A:
                var customerChange3602ADoc = await _documentDataStorage.FirstOrDefaultByEntityId<CustomerChange3602Data>(model.SalesArrangementId, cancellation);
                model.CustomerChange3602A = customerChange3602ADoc?.Data?.MapCustomerChange3602();
                break;

            case SalesArrangementTypes.CustomerChange3602B:
                var customerChange3602BDoc = await _documentDataStorage.FirstOrDefaultByEntityId<CustomerChange3602Data>(model.SalesArrangementId, cancellation);
                model.CustomerChange3602B = customerChange3602BDoc?.Data?.MapCustomerChange3602();
                break;

            case SalesArrangementTypes.CustomerChange3602C:
                var customerChange3602CDoc = await _documentDataStorage.FirstOrDefaultByEntityId<CustomerChange3602Data>(model.SalesArrangementId, cancellation);
                model.CustomerChange3602C = customerChange3602CDoc?.Data?.MapCustomerChange3602();
                break;

            default:
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.SATypeNotSupported, model.SalesArrangementTypeId);
        }

        return model;
    }

    private readonly SalesArrangementServiceDbContext _dbContext;
    private readonly IDocumentDataStorage _documentDataStorage;

    public GetSalesArrangementHandler(SalesArrangementServiceDbContext dbContext, IDocumentDataStorage documentDataStorage)
    {
        _dbContext = dbContext;
        _documentDataStorage = documentDataStorage;
    }
}
