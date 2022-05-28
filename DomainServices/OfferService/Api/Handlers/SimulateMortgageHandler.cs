using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;
using Grpc.Core;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.CisTypes;

namespace DomainServices.OfferService.Api.Handlers;

internal class SimulateMortgageHandler
    : BaseHandler, IRequestHandler<Dto.SimulateMortgageMediatrRequest, SimulateMortgageResponse>
{
    #region Construction

    private readonly ILogger<SimulateMortgageHandler> _logger;
    private readonly Eas.IEasClient _easClient;
    private readonly EasSimulationHT.IEasSimulationHTClient _easSimulationHTClient;

    public SimulateMortgageHandler(
        Repositories.OfferRepository repository,
        ILogger<SimulateMortgageHandler> logger,
        ICodebookServiceAbstraction codebookService,
        Eas.IEasClient easClient,
        EasSimulationHT.IEasSimulationHTClient easSimulationHTClient
        ) : base(repository, codebookService)
    {
        _logger = logger;
        _easClient = easClient;
        _easSimulationHTClient = easSimulationHTClient;
    }

    #endregion

    public async Task<SimulateMortgageResponse> Handle(Dto.SimulateMortgageMediatrRequest request, CancellationToken cancellation)
    {

        // kontrola ProductTypeId (zda je typu Mortgage)
        await CheckProductTypeCategory(
            request.Request.SimulationInputs.ProductTypeId,
            CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeCategory.Mortgage
        );

        var resourceProcessId = Guid.Parse(request.Request.ResourceProcessId);

        var basicParameters = request.Request.BasicParameters;

        var inputs = request.Request.SimulationInputs;

        // get simulation outputs
        var easSimulationReq = inputs.ToEasSimulationRequest();
        var easSimulationRes = ResolveRunSimulationHT(await _easSimulationHTClient.RunSimulationHT(easSimulationReq));
        var results = easSimulationRes.ToSimulationResults();

        // save to DB
        var entity = await _repository.SaveOffer(resourceProcessId, basicParameters, inputs, results, cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.Offer), entity.OfferId);

        // create response
        return new SimulateMortgageResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new ModificationStamp(entity),
            BasicParameters = basicParameters,
            SimulationInputs = inputs,
            SimulationResults = results,
        };

    }

    private static ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper.SimulationHTResponse ResolveRunSimulationHT(IServiceCallResult result) =>
       result switch
       {
           SuccessfulServiceCallResult<ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper.SimulationHTResponse> r => r.Model,
           ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors[0].Message, err.Errors[0].Key),
           _ => throw new NotImplementedException("RunSimulationHT")
       };
}

