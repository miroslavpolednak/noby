using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Clients;
using Microsoft.EntityFrameworkCore;
using Grpc.Core;
using CIS.Infrastructure.gRPC;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOfferFPSchedule;

internal sealed class GetMortgageOfferFPScheduleHandler
    : IRequestHandler<GetMortgageOfferFPScheduleRequest, GetMortgageOfferFPScheduleResponse>
{
    public async Task<GetMortgageOfferFPScheduleResponse> Handle(GetMortgageOfferFPScheduleRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.Offers
           .AsNoTracking()
           .Where(t => t.OfferId == request.OfferId)
           .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(10000, $"Offer #{request.OfferId} not found");

        // load codebook DrawingDuration for remaping Id -> DrawingDuration
        var drawingDurationsById = (await _codebookService.DrawingDurations(cancellation)).ToDictionary(i => i.Id);

        // load codebook DrawingType for remaping Id -> StarbildId
        var drawingTypeById = (await _codebookService.DrawingTypes(cancellation)).ToDictionary(i => i.Id);

        var basicParameters = BasicParameters.Parser.ParseFrom(entity.BasicParametersBin);
        var inputs = MortgageSimulationInputs.Parser.ParseFrom(entity.SimulationInputsBin);

        // get simulation outputs
        var easSimulationReq = inputs.ToEasSimulationRequest(basicParameters, drawingDurationsById, drawingTypeById).ToEasSimulationFullPaymentScheduleRequest();
        var easSimulationRes = resolveRunSimulationHT(await _easSimulationHTClient.RunSimulationHT(easSimulationReq));

        var fullPaymentSchedule = easSimulationRes.ToFullPaymentSchedule();

        var model = new GetMortgageOfferFPScheduleResponse { };
        model.PaymentScheduleFull.Add(fullPaymentSchedule);

        return model;
    }

    private static EasSimulationHT.EasSimulationHTWrapper.SimulationHTResponse resolveRunSimulationHT(IServiceCallResult result) =>
      result switch
      {
          SuccessfulServiceCallResult<EasSimulationHT.EasSimulationHTWrapper.SimulationHTResponse> r => r.Model,
          ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors[0].Message, err.Errors[0].Key),
          _ => throw new NotImplementedException("RunSimulationHT")
      };

    private readonly Database.OfferServiceDbContext _dbContext;
    private readonly ICodebookServiceClients _codebookService;
    private readonly EasSimulationHT.IEasSimulationHTClient _easSimulationHTClient;

    public GetMortgageOfferFPScheduleHandler(
        Database.OfferServiceDbContext dbContext, ICodebookServiceClients codebookService, EasSimulationHT.IEasSimulationHTClient easSimulationHTClient)
    {
        _dbContext = dbContext;
        _codebookService = codebookService;
        _easSimulationHTClient = easSimulationHTClient;
    }
}