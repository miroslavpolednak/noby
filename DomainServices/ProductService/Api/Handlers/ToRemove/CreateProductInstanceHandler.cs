//using CIS.Core.Results;
//using CIS.Infrastructure.gRPC;
//using DomainServices.ProductService.Contracts;
//using Grpc.Core;

//namespace DomainServices.ProductService.Api.Handlers;

//internal class CreateProductInstanceHandler
//    : IRequestHandler<Dto.CreateProductInstanceMediatrRequest, CreateProductInstanceResponse>
//{
//    public async Task<CreateProductInstanceResponse> Handle(Dto.CreateProductInstanceMediatrRequest request, CancellationToken cancellation)
//    {
//        _logger.LogInformation("Create product with Case ID #{id}", request.CaseId);

//        return new CreateProductInstanceResponse()
//        {
//            ProductInstanceId = 1
//        };

//        // ID noveho produktu se vetsinou rovna ID Case
//        var productId = request.CaseId;

//        // caseId musi existovat
//        if (!await _repository.IsExistingCase(request.CaseId))
//            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Case ID #{request.CaseId} does not exist", 12000);

//        // zjistit o jakou kategorii produktu se jedna z daneho typu produktu - SS, Uver SS, Hypoteka
//        var productInstanceTypeCategory = await getProductCategory(request.ProductInstanceTypeId);

//        // Pokud typ produktu (ProductInstanceTypeId = HS_LOAN) jedná se o úvěr navazující na existující stavební spoření
//        // Rezervace ID produktu v EAS (uver_id)
//        if (productInstanceTypeCategory == CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeCategory.BuildingSavings)
//            productId = resolveSavingsLoanIdResult(await _easClient.GetSavingsLoanId(request.CaseId));

//        // vytvoreni produktu v MpHome
//        resolveCreateProductResult(await createProduct(productInstanceTypeCategory, request.CaseId, productId));

//        return new CreateProductInstanceResponse() 
//        { 
//            ProductInstanceId = productId 
//        };
//    }

//    private async Task<CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeCategory> getProductCategory(long ProductInstanceTypeId)
//    {
//        var productTypes = await _codebookService.ProductTypes();
//        var item = productTypes.FirstOrDefault(t => t.Id == ProductInstanceTypeId);
//        if (item == null)
//            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, "ProductInstanceTypeId not found", 1);
//        return item.ProductCategory;
//    }

//    private async Task<IServiceCallResult> createProduct(CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeCategory category, long caseId, long productId) =>
//        category switch
//        {
//            /*CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeCategory.BuildingSavings => await _mpHomeClient.UpdateSavings(caseId),
//            CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeCategory.BuildingSavingsLoan => await _mpHomeClient.CreateSavingsLoanInstance(productId),
//            CodebookService.Contracts.Endpoints.ProductInstanceTypes.ProductInstanceTypeCategory.Mortgage => await _mpHomeClient.CreateMorgageInstance(productId),*/
//            _ => throw new NotImplementedException()
//        };

//    private bool resolveCreateProductResult(IServiceCallResult result) =>
//        result switch
//        {
//            SuccessfulServiceCallResult r => true,
//            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key),
//            _ => throw new NotImplementedException()
//        };

//    private long resolveSavingsLoanIdResult(IServiceCallResult result) =>
//        result switch
//        {
//            SuccessfulServiceCallResult<long> r when r.Model > 0 => r.Model,
//            SuccessfulServiceCallResult<long> r when r.Model == 0 => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, "Unable to create MktItem instance in Starbuild.", 12002),
//            ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key),
//            _ => throw new NotImplementedException()
//        };

//    private readonly Repositories.NobyDbRepository _repository;
//    private readonly ILogger<CreateProductInstanceHandler> _logger;
//    private readonly Eas.IEasClient _easClient;
//    private readonly ExternalServices.MpHome.V1.IMpHomeClient _mpHomeClient;
//    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;

//    public CreateProductInstanceHandler(
//        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
//        Eas.IEasClient easClient,
//        ExternalServices.MpHome.V1.IMpHomeClient mpHomeClient,
//        Repositories.NobyDbRepository repository,
//        ILogger<CreateProductInstanceHandler> logger)
//    {
//        _mpHomeClient = mpHomeClient;
//        _easClient = easClient;
//        _repository = repository;
//        _logger = logger;
//        _codebookService = codebookService;
//    }
//}
