using Grpc.Core;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;
using _Offer = DomainServices.OfferService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;
using Azure.Core;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class LinkModelationToSalesArrangementHandler
    : IRequestHandler<Dto.LinkModelationToSalesArrangementMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.LinkModelationToSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        // overit existenci SA
        var salesArrangementInstance = await _dbContext.SalesArrangements.FindAsync(new object[] { request.SalesArrangementId }, cancellation) 
            ?? throw new CisNotFoundException(17000, $"Sales arrangement ID {request.SalesArrangementId} does not exist.");
        
        // kontrola zda SA uz neni nalinkovan na stejnou Offer na kterou je request
        if (salesArrangementInstance.OfferId == request.OfferId)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"SalesArrangement {request.SalesArrangementId} is already linked to Offer {request.OfferId}", 16012);

        // validace na existenci offer
        var offerInstance = ServiceCallResult.ResolveToDefault<_Offer.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(request.OfferId, cancellation))
            ?? throw new CisNotFoundException(17001, $"Offer ID #{request.OfferId} does not exist.");

        // kontrola, zda simulace neni nalinkovana na jiny SA
        if (await _dbContext.SalesArrangements.AnyAsync(t => t.OfferId == request.OfferId, cancellation))
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"Offer {request.OfferId} is already linked to another SA", 16057);

        // Kontrola, že nová Offer má GuaranteeDateFrom větší nebo stejné jako původně nalinkovaná offer
        if (salesArrangementInstance.OfferId.HasValue)
        {
            var offerInstanceOld = ServiceCallResult.ResolveToDefault<_Offer.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(salesArrangementInstance.OfferId.Value, cancellation))
                ?? throw new CisNotFoundException(17001, $"Offer ID #{salesArrangementInstance.OfferId} does not exist.");
            if ((DateTime)offerInstance.SimulationInputs.GuaranteeDateFrom < (DateTime)offerInstanceOld.SimulationInputs.GuaranteeDateFrom)
                throw new CisValidationException(16058, $"Old offer GuaranteeDateFrom > than new GuaranteeDateFrom");
        }

        // update linku v DB
        salesArrangementInstance.OfferGuaranteeDateFrom = offerInstance.SimulationInputs.GuaranteeDateFrom;
        salesArrangementInstance.OfferGuaranteeDateTo = offerInstance.BasicParameters.GuaranteeDateTo;
        salesArrangementInstance.OfferId = request.OfferId;
        salesArrangementInstance.ResourceProcessId = Guid.Parse(offerInstance.ResourceProcessId);

        await _dbContext.SaveChangesAsync(cancellation);

        // update parametru
        await updateParameters(salesArrangementInstance, offerInstance, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private async Task updateParameters(Repositories.Entities.SalesArrangement salesArrangementInstance, _Offer.GetMortgageOfferResponse offerInstance, CancellationToken cancellation)
    {
        // parametry SA
        bool hasChanged = false;
        var saParameters = (await _dbContext.SalesArrangementsParameters.FirstOrDefaultAsync(t => t.SalesArrangementId == salesArrangementInstance.SalesArrangementId, cancellation));
        if (saParameters is null)
        {
            saParameters = new Repositories.Entities.SalesArrangementParameters();
            _dbContext.SalesArrangementsParameters.Add(saParameters);
        }
        var parametersModel = saParameters?.ParametersBin is not null ? _SA.SalesArrangementParametersMortgage.Parser.ParseFrom(saParameters.ParametersBin) : new _SA.SalesArrangementParametersMortgage();

        if (offerInstance.SimulationInputs.LoanKindId == 2001 ||
            (offerInstance.SimulationInputs.LoanPurposes is not null && offerInstance.SimulationInputs.LoanPurposes.Any(t => t.LoanPurposeId == 201)))
        {
            parametersModel.LoanRealEstates.Clear();
            hasChanged = true;
        }

        // HFICH-2181
        if (parametersModel.ExpectedDateOfDrawing != null || (parametersModel.ExpectedDateOfDrawing == null && offerInstance.SimulationInputs.ExpectedDateOfDrawing > DateTime.Now.AddDays(1)))
        {
            parametersModel.ExpectedDateOfDrawing = offerInstance.SimulationInputs.ExpectedDateOfDrawing;
            hasChanged = true;
        }

        if (hasChanged)
        {
            saParameters!.Parameters = Newtonsoft.Json.JsonConvert.SerializeObject(parametersModel);
            saParameters.ParametersBin = parametersModel.ToByteArray();
            await _dbContext.SaveChangesAsync(cancellation);
        }
    }

    private readonly OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;

    public LinkModelationToSalesArrangementHandler(
        Repositories.SalesArrangementServiceDbContext dbContext,
        OfferService.Abstraction.IOfferServiceAbstraction offerService)
    {
        _dbContext = dbContext;
        _offerService = offerService;
    }
}
