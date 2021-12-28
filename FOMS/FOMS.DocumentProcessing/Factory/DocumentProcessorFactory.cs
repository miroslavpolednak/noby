using CIS.Core.Results;
using Microsoft.AspNetCore.Http;

namespace FOMS.DocumentProcessing;

public sealed class DocumentProcessorFactory : IDocumentProcessorFactory
{
    public async Task<IDocumentProcessor> CreateDocumentProcessor(int salesArrangementId)
    {
        var salesArrangement = resolveResult(await _salesArrangementService.GetSalesArrangement(salesArrangementId));

        // zatim switchem, az budu vedet jakym zpusobem se s SA pracuje, tak udelame nejakou automatiku
        switch (salesArrangement.SalesArrangementType)
        {
            case 1:
                return new HousingSavingsProcessor(_serviceAccessor, salesArrangement);
            default:
                throw new NotImplementedException($"Document Processor for {salesArrangement.SalesArrangementType} not found");
        }
        
    }

    private DomainServices.SalesArrangementService.Contracts.GetSalesArrangementResponse resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.SalesArrangementService.Contracts.GetSalesArrangementResponse> r => r.Model,
            _ => throw new NotImplementedException()
        };

    private readonly ServiceAccessor _serviceAccessor;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;

    public DocumentProcessorFactory(IHttpContextAccessor httpContext, DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
        _serviceAccessor = new ServiceAccessor(httpContext);
    }
}
