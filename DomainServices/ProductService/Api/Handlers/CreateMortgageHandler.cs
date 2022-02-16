using DomainServices.ProductService.Contracts;
using ExternalServices.MpHome.V1._1;
using DomainServices.CodebookService.Abstraction;
using CIS.Infrastructure.gRPC;

namespace DomainServices.ProductService.Api.Handlers;

internal class CreateMortgageHandler
    : IRequestHandler<Dto.CreateMortgageMediatrRequest, ProductIdReqRes>
{
    #region Construction

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly IMpHomeClient _mpHomeClient;
    private readonly ILogger<CreateMortgageHandler> _logger;
    
    public CreateMortgageHandler(
        ICodebookServiceAbstraction codebookService,
        IMpHomeClient mpHomeClient,
        ILogger<CreateMortgageHandler> logger)
    {
        _codebookService = codebookService;
        _mpHomeClient = mpHomeClient;
        _logger = logger;
    }

    #endregion

    public async Task<ProductIdReqRes> Handle(Dto.CreateMortgageMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(CreateMortgageHandler));

        // TODO: check if mortgage exists
        // TODO: check product type id (Mortgage)

        var mortgageRequest = request.Request.Mortgage.ToMortgageRequest();

        var updateLoanResult = await _mpHomeClient.UpdateLoan(request.Request.CaseId, mortgageRequest);

        

        if (!updateLoanResult.Success)
        {
            //throw GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key)
            throw GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.Internal, nameof(CreateMortgageHandler), 0);
        }

        //updateLoanResult switch
        //{
        //    SuccessfulServiceCallResult r => true,
        //    ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key),
        //    _ => throw new NotImplementedException()
        //};

        var model = new ProductIdReqRes
        {
           ProductId = request.Request.CaseId,
        };

        return model;
    }
  
}