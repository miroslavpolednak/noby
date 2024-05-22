using DomainServices.CaseService.Clients.v1;
using DomainServices.OfferService.Api.Database;
using DomainServices.OfferService.Contracts;
using KafkaFlow;
using Microsoft.EntityFrameworkCore;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.OfferService.Api.Messaging.LoanRetentionProcessChanged;

internal sealed class LoanRetentionProcessChangedHandler : IMessageHandler<cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.LoanRetentionProcessChanged>
{
    private readonly ICaseServiceClient _caseService;
    private readonly OfferServiceDbContext _dbContext;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly ILogger<LoanRetentionProcessChangedHandler> _logger;

    public LoanRetentionProcessChangedHandler(ICaseServiceClient caseService, 
                                              OfferServiceDbContext dbContext,
                                              IDocumentDataStorage documentDataStorage,
                                              ILogger<LoanRetentionProcessChangedHandler> logger)
    {
        _caseService = caseService;
        _dbContext = dbContext;
        _documentDataStorage = documentDataStorage;
        _logger = logger;
    }

    public async Task Handle(IMessageContext context, cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.LoanRetentionProcessChanged message)
    {
        if (message.state is not (cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.COMPLETED
            or cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1.ProcessStateEnum.TERMINATED))
            return;

        if (!long.TryParse(message.@case.caseId.id, out var caseId))
        {
            _logger.KafkaMessageCaseIdIncorrectFormat(nameof(LoanRetentionProcessChangedHandler), message.@case.caseId.id);
            return;
        }

        if (!long.TryParse(message.id, out var processId))
        {
            _logger.KafkaMessageCurrentTaskIdIncorrectFormat(nameof(LoanRetentionProcessChangedHandler), message.id);
            return;
        }

        var caseIdValidation = await _caseService.ValidateCaseId(caseId);

        if (!caseIdValidation.Exists)
        {
            _logger.KafkaCaseIdNotFound(nameof(LoanRetentionProcessChangedHandler), caseId);
            return;
        }

        var processes = await _caseService.GetProcessList(caseId);
        var refixationProcess = processes.FirstOrDefault(p => p.ProcessId == processId && p.RefinancingType == (int)RefinancingTypes.MortgageRefixation);

        if (refixationProcess is null)
            return;

        var refixationOffers = await _dbContext.Offers
                                               .Where(o => o.CaseId == caseId
                                                           && o.OfferType == (int)OfferTypes.MortgageRefixation && o.ValidTo > DateTime.UtcNow.ToLocalTime()
                                                           && !((OfferFlagTypes)o.Flags).HasFlag(OfferFlagTypes.LegalNotice))
                                               .ToListAsync();

        var offersData = (await _documentDataStorage.GetList<Database.DocumentDataEntities.MortgageRefixationData, int>(refixationOffers.Select(o => o.OfferId).ToArray())).ToDictionary(k => k.EntityId);

        foreach (var offer in refixationOffers)
        {
            offer.Flags = RemoveOfferFlags((OfferFlagTypes)offer.Flags);

            var offerDataObj = offersData[offer.OfferId].Data;

            offerDataObj!.SimulationInputs.InterestRateDiscount = null;
            offerDataObj.SimulationOutputs.LoanPaymentAmountDiscounted = offerDataObj.SimulationOutputs.LoanPaymentAmount;

            await _documentDataStorage.Update(offersData[offer.OfferId].DocumentDataStorageId, offerDataObj);
        }

        await _dbContext.SaveChangesAsync();
    }

    private static int RemoveOfferFlags(OfferFlagTypes flags)
    {
        flags &= ~(OfferFlagTypes.Selected | OfferFlagTypes.Liked | OfferFlagTypes.Communicated);

        return (int)flags;
    }
}