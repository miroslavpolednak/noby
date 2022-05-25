﻿using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using _Offer = DomainServices.OfferService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class LinkModelationToSalesArrangementHandler
    : IRequestHandler<Dto.LinkModelationToSalesArrangementMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.LinkModelationToSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        // overit existenci SA
        var salesArrangementInstance = await _dbContext.SalesArrangements.FindAsync(new object[] { request.SalesArrangementId }, cancellation) 
            ?? throw new CisNotFoundException(16000, $"Sales arrangement ID {request.SalesArrangementId} does not exist.");
        
        // kontrola zda SA uz neni nalinkovan na stejnou Offer na kterou je request
        if (salesArrangementInstance.OfferId == request.OfferId)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"SalesArrangement {request.SalesArrangementId} is already linked to Offer {request.OfferId}", 16012);

        // validace na existenci offer
        var offerInstance = ServiceCallResult.ResolveToDefault<_Offer.GetMortgageDataResponse>(await _offerService.GetMortgageData(request.OfferId, cancellation))
            ?? throw new CisNotFoundException(16001, $"Offer ID #{request.OfferId} does not exist.");

        // kontrola, zda simulace neni nalinkovana na jiny SA
        if (await _dbContext.SalesArrangements.AnyAsync(t => t.OfferId == request.OfferId, cancellation))
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"Offer {request.OfferId} is already linked to another SA", 16012);

        if (offerInstance.Inputs.LoanKindId == 2001 || 
            (offerInstance.Inputs.LoanPurpose is not null && offerInstance.Inputs.LoanPurpose.Any(t => t.LoanPurposeId == 201)))
        {
            var saParameters = (await _dbContext.SalesArrangementsParameters.FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation));
            if (!string.IsNullOrEmpty(saParameters?.Parameters))
            {
                var parametersModel = JsonSerializer.Deserialize<_SA.SalesArrangementParametersMortgage>(saParameters.Parameters!)!;
                parametersModel.LoanRealEstates.Clear();
                saParameters.Parameters = JsonSerializer.Serialize(parametersModel);
                await _dbContext.SaveChangesAsync(cancellation); //ma se to ulozit hned
            }
        }

        // Kontrola, že nová Offer má GuaranteeDateFrom větší nebo stejné jako původně nalinkovaná offer
        if (salesArrangementInstance.OfferId.HasValue)
        {
            var offerInstanceOld = ServiceCallResult.ResolveToDefault<_Offer.GetMortgageDataResponse>(await _offerService.GetMortgageData(salesArrangementInstance.OfferId.Value, cancellation))
                ?? throw new CisNotFoundException(16001, $"Offer ID #{salesArrangementInstance.OfferId} does not exist.");
            if (offerInstance.Inputs.GuaranteeDateFrom < offerInstanceOld.Inputs.GuaranteeDateFrom)
                throw new CisValidationException(16000, $"Old offer GuaranteeDateFrom > than new GuaranteeDateFrom");
        }

        // update linku v DB
        salesArrangementInstance.OfferGuaranteeDateFrom = offerInstance.Inputs.GuaranteeDateFrom;
        salesArrangementInstance.OfferGuaranteeDateTo = offerInstance.Outputs.OfferGuaranteeDateTo;
        salesArrangementInstance.OfferId = request.OfferId;
        salesArrangementInstance.ResourceProcessId = Guid.Parse(offerInstance.ResourceProcessId);

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<LinkModelationToSalesArrangementHandler> _logger;

    public LinkModelationToSalesArrangementHandler(
        Repositories.SalesArrangementServiceDbContext dbContext,
        OfferService.Abstraction.IOfferServiceAbstraction offerService,
        ILogger<LinkModelationToSalesArrangementHandler> logger)
    {
        _dbContext = dbContext;
        _offerService = offerService;
        _logger = logger;
    }
}
