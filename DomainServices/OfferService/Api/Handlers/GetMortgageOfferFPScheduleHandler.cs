﻿using _OS = DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;

using Microsoft.EntityFrameworkCore;

using Grpc.Core;
using CIS.Infrastructure.gRPC;


namespace DomainServices.OfferService.Api.Handlers;

internal class GetMortgageOfferFPScheduleHandler
    : IRequestHandler<Dto.GetMortgageOfferFPScheduleMediatrRequest, _OS.GetMortgageOfferFPScheduleResponse>
{
    #region Construction

    private readonly Repositories.OfferServiceDbContext _dbContext;
    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly EasSimulationHT.IEasSimulationHTClient _easSimulationHTClient;

    public GetMortgageOfferFPScheduleHandler(
        Repositories.OfferServiceDbContext dbContext, ICodebookServiceAbstraction codebookService, EasSimulationHT.IEasSimulationHTClient easSimulationHTClient)
    {
        _dbContext = dbContext;
        _codebookService = codebookService;
        _easSimulationHTClient = easSimulationHTClient;
    }

    #endregion

    public async Task<_OS.GetMortgageOfferFPScheduleResponse> Handle(Dto.GetMortgageOfferFPScheduleMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.Offers
           .AsNoTracking()
           .Where(t => t.OfferId == request.OfferId)
           .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(10000, $"Offer #{request.OfferId} not found");

        // load codebook DrawingDuration for remaping Id -> DrawingDuration
        var drawingDurationsById = (await _codebookService.DrawingDurations(cancellation)).ToDictionary(i => i.Id);

        // load codebook DrawingType for remaping Id -> StarbildId
        var drawingTypeById = (await _codebookService.DrawingTypes(cancellation)).ToDictionary(i => i.Id);

        var basicParameters = _OS.BasicParameters.Parser.ParseFrom(entity.BasicParametersBin);
        var inputs = _OS.MortgageSimulationInputs.Parser.ParseFrom(entity.SimulationInputsBin);

        // get simulation outputs
        var easSimulationReq = inputs.ToEasSimulationRequest(basicParameters, drawingDurationsById, drawingTypeById).ToEasSimulationFullPaymentScheduleRequest();
        var easSimulationRes = resolveRunSimulationHT(await _easSimulationHTClient.RunSimulationHT(easSimulationReq));

        var fullPaymentSchedule = easSimulationRes.ToFullPaymentSchedule();
        
        var model = new _OS.GetMortgageOfferFPScheduleResponse{};
        model.PaymentScheduleFull.Add(fullPaymentSchedule);

        return model;
    }

    private static ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper.SimulationHTResponse resolveRunSimulationHT(IServiceCallResult result) =>
      result switch
      {
          SuccessfulServiceCallResult<ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper.SimulationHTResponse> r => r.Model,
          ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors[0].Message, err.Errors[0].Key),
          _ => throw new NotImplementedException("RunSimulationHT")
      };

}