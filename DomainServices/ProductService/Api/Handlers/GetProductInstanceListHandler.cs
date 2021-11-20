using CIS.Infrastructure.gRPC;
using DomainServices.ProductService.Contracts;
using Grpc.Core;
using System.Linq;
using ProdTypes = DomainServices.CodebookService.Contracts.Endpoints.ProductInstanceTypes;

namespace DomainServices.ProductService.Api.Handlers;

internal class GetProductInstanceListHandler
    : IRequestHandler<Dto.GetProductInstanceListMediatrRequest, GetProductInstanceListResponse>
{
    public async Task<GetProductInstanceListResponse> Handle(Dto.GetProductInstanceListMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get list for Case ID #{id}", request.CaseId);
        List<ProdTypes.ProductInstanceTypeItem> productTypes = await _codebookService.ProductInstanceTypes();

        // ziskat instanci CASE
        var caseInstance = await _nobyRepository.GetCase(request.CaseId) ?? throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Case ID #{request.CaseId} does not exist", 12000);
        
        var model = new GetProductInstanceListResponse();
        switch (productTypes.First(t => t.Id == caseInstance.ProductInstanceType).ProductCategory)
        {
            // nemusi tu byt kontrola na existenci, protoze SS tam vzdy je
            case ProdTypes.ProductInstanceTypeCategory.BuildingSavings:
                model.Instances.Add(await getSavingsListItem(caseInstance.CaseId, productTypes));
                break;

            case ProdTypes.ProductInstanceTypeCategory.BuildingSavingsLoan:
                // SS
                model.Instances.Add(await getSavingsListItem(caseInstance.CaseId, productTypes));
                // uvery
                model.Instances.AddRange(await getSavingsLoanListItems(caseInstance.CaseId, productTypes));
                break;

            default:
                throw new NotImplementedException();
        }
        
        model.Instances.Add(new ProductInstanceListItem { ProductInstanceId = 1, ProductInstanceState = 1, ProductInstanceType = 1 });

        return model;
    }

    private async Task<ProductInstanceListItem> getSavingsListItem(long caseId, List<ProdTypes.ProductInstanceTypeItem> productTypes)
    {
        var instance = (await _konsRepository.GetSavingsListItem(caseId)) ?? throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Product ID #{caseId} does not exist", 12000);
        return instance.CreateContractItem(productTypes);
    }

    private async Task<List<ProductInstanceListItem>> getSavingsLoanListItems(long caseId, List<ProdTypes.ProductInstanceTypeItem> productTypes)
    {
        //TODO musi tady byt vzdy aspon jeden?
        var instances = await _konsRepository.GetSavingsLoanListItems(caseId);
        return instances.Select(t => t.CreateContractItem(productTypes)).ToList();
    }

    private readonly ILogger<GetProductInstanceListHandler> _logger;
    private readonly Repositories.NobyDbRepository _nobyRepository;
    private readonly Repositories.KonsDbRepository _konsRepository;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

    public GetProductInstanceListHandler(
        Repositories.KonsDbRepository konsRepository,
        Repositories.NobyDbRepository nobyRepository,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        ILogger<GetProductInstanceListHandler> logger)
    {
        _konsRepository = konsRepository;
        _nobyRepository = nobyRepository;
        _logger = logger;
        _codebookService = codebookService;
    }
}

