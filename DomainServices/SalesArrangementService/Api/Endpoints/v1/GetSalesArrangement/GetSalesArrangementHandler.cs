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
                var mortgageData = await GetParametersData<MortgageData>(model.SalesArrangementId, cancellation);
                model.Mortgage = mortgageData?.MapMortgage();
                break;

            case SalesArrangementTypes.Drawing:
                var drawingData = await GetParametersData<DrawingData>(model.SalesArrangementId, cancellation);
                model.Drawing = drawingData?.MapDrawing();
                break;

            case SalesArrangementTypes.GeneralChange:
                var generalChangeData = await GetParametersData<GeneralChangeData>(model.SalesArrangementId, cancellation);
                model.GeneralChange = generalChangeData?.MapGeneralChange();
                break;

            case SalesArrangementTypes.HUBN:
                var hubnData = await GetParametersData<HUBNData>(model.SalesArrangementId, cancellation);
                model.HUBN = hubnData?.MapHUBN();
                break;

            case SalesArrangementTypes.CustomerChange:
                var customerChangeData = await GetParametersData<CustomerChangeData>(model.SalesArrangementId, cancellation);
                model.CustomerChange = customerChangeData?.MapCustomerChange();
                break;

            case SalesArrangementTypes.CustomerChange3602A:
                var customerChange3602AData = await GetParametersData<CustomerChange3602Data>(model.SalesArrangementId, cancellation);
                model.CustomerChange3602A = customerChange3602AData?.MapCustomerChange3602();
                break;

            case SalesArrangementTypes.CustomerChange3602B:
                var customerChange3602BData = await GetParametersData<CustomerChange3602Data>(model.SalesArrangementId, cancellation);
                model.CustomerChange3602B = customerChange3602BData?.MapCustomerChange3602();
                break;

            case SalesArrangementTypes.CustomerChange3602C:
                var customerChange3602CData = await GetParametersData<CustomerChange3602Data>(model.SalesArrangementId, cancellation);
                model.CustomerChange3602C = customerChange3602CData?.MapCustomerChange3602();
                break;

            case SalesArrangementTypes.MortgageRetention:
                var retentionData = await GetParametersData<RetentionData>(model.SalesArrangementId, cancellation);
                model.Retention = retentionData?.MapRetention();
                break;

            case SalesArrangementTypes.MortgageRefixation:
                var refixationData = await GetParametersData<RefixationData>(model.SalesArrangementId, cancellation);
                model.Refixation = refixationData?.MapRefixation();
                break;

            case SalesArrangementTypes.MortgageExtraPayment:
                var extraPaymentData = await GetParametersData<ExtraPaymentData>(model.SalesArrangementId, cancellation);
                model.ExtraPayment = extraPaymentData?.MapExtraPayment();
                break;

            default:
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.SATypeNotSupported, model.SalesArrangementTypeId);
        }

        return model;
    }

    private async Task<TData?> GetParametersData<TData>(int salesArrangementId, CancellationToken cancellationToken) 
        where TData : class, IDocumentData
    {
        var documentData = await _documentDataStorage.FirstOrDefaultByEntityId<TData, int>(salesArrangementId, SalesArrangementParametersConst.TableName, cancellationToken);

        return documentData?.Data;
    }

    private readonly SalesArrangementServiceDbContext _dbContext;
    private readonly IDocumentDataStorage _documentDataStorage;

    public GetSalesArrangementHandler(SalesArrangementServiceDbContext dbContext, IDocumentDataStorage documentDataStorage)
    {
        _dbContext = dbContext;
        _documentDataStorage = documentDataStorage;
    }
}
