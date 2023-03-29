using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Clients;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOfferFPSchedule;

internal sealed class GetMortgageOfferFPScheduleHandler
    : IRequestHandler<GetMortgageOfferFPScheduleRequest, GetMortgageOfferFPScheduleResponse>
{
    public async Task<GetMortgageOfferFPScheduleResponse> Handle(GetMortgageOfferFPScheduleRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .Offers
            .AsNoTracking()
            .Where(t => t.OfferId == request.OfferId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);

        // load codebook DrawingDuration for remaping Id -> DrawingDuration
        var drawingDurationsById = (await _codebookService.DrawingDurations(cancellationToken)).ToDictionary(i => i.Id);

        // load codebook DrawingType for remaping Id -> StarbildId
        var drawingTypeById = (await _codebookService.DrawingTypes(cancellationToken)).ToDictionary(i => i.Id);

        var basicParameters = BasicParameters.Parser.ParseFrom(entity.BasicParametersBin);
        var inputs = MortgageSimulationInputs.Parser.ParseFrom(entity.SimulationInputsBin);

        // get simulation outputs
        var easSimulationReq = inputs.ToEasSimulationRequest(basicParameters, drawingDurationsById, drawingTypeById).ToEasSimulationFullPaymentScheduleRequest();
        var easSimulationRes = await _easSimulationHTClient.RunSimulationHT(easSimulationReq, cancellationToken);

        var fullPaymentSchedule = easSimulationRes.ToFullPaymentSchedule();

        var model = new GetMortgageOfferFPScheduleResponse { };
        model.PaymentScheduleFull.Add(fullPaymentSchedule);

        return model;
    }

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