using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;
using Google.Protobuf;
using System.Threading;

namespace DomainServices.RealEstateValuationService.Api.Services;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class OrderAggregate
{
    public async Task<(Database.Entities.RealEstateValuation REVEntity, long[]? RealEstateIds, long[]? Attachments, CaseService.Contracts.Case Case)> GetAggregatedData(int realEstateValuationId, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .RealEstateValuations
            .FirstOrDefaultAsync(t => t.RealEstateValuationId == realEstateValuationId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, realEstateValuationId);

        var deedOfOwnerships = await _dbContext
            .DeedOfOwnershipDocuments
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == realEstateValuationId)
            .Select(t => new { t.RealEstateIds })
            .ToListAsync(cancellationToken);

        var attachments = await _dbContext
            .Attachments
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == realEstateValuationId)
            .Select(t => t.ExternalId)
            .ToArrayAsync(cancellationToken);

        // realestateids
        var realEstateIds = deedOfOwnerships
            .Where(t => !string.IsNullOrEmpty(t.RealEstateIds))
            .SelectMany(t =>
            {
                return System.Text.Json.JsonSerializer.Deserialize<long[]>(t.RealEstateIds!)!;
            })
            .ToArray();

        // case detail
        var caseInstance = await _caseService.GetCaseDetail(entity.CaseId, cancellationToken);
        
        return (entity, realEstateIds, attachments, caseInstance);
    }

    public async Task SaveResults(Database.Entities.RealEstateValuation entity, long orderId, OrdersStandard? data, CancellationToken cancellationToken)
    {
        // ulozeni vysledku
        entity.OrderId = orderId;
        entity.ValuationSentDate = _dbContext.CisDateTime.Now;
        entity.ValuationStateId = (int)RealEstateValuationStates.Dokonceno;

        // if revaluation
        if (entity.IsRevaluationRequired && data is not null)
        {
            var orderEntity = new Database.Entities.RealEstateValuationOrder
            {
                RealEstateValuationId = entity.RealEstateValuationId,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(data),
                DataBin = data.ToByteArray()
            };
            _dbContext.RealEstateValuationOrders.Add(orderEntity);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;
    private readonly ICaseServiceClient _caseService;
    
    public OrderAggregate(
        RealEstateValuationServiceDbContext dbContext,
        ICaseServiceClient caseService)
    {
        _caseService = caseService;
        _dbContext = dbContext;
    }
}
