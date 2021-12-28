using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DomainServices.OfferService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class SimulateBuildingSavingsRepository
{
    private readonly OfferServiceDbContext _dbContext;

    public SimulateBuildingSavingsRepository(OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveOffer(
        Guid resourceProcessId,
        int productInstanceType,
        BuildingSavingsInput inputs,
        Dto.Models.BuildingSavingsDataModel outputs)
    {
        var entity = new Entities.OfferInstance
        {
            ResourceProcessId = resourceProcessId,
            ProductInstanceType = productInstanceType,
            Outputs = JsonSerializer.Serialize(outputs),
            Inputs = JsonSerializer.Serialize(inputs)
        };

        _dbContext.OfferModelations.Add(entity);

        await _dbContext.SaveChangesAsync();

        return entity.OfferInstanceId;
    }

    public async Task SaveSchedules(int offerInstanceId, List<ScheduleItem>? scheduleItems)
    {
        var entity = new Entities.ScheduleItems
        {
            OfferInstanceId = offerInstanceId,
            Data = JsonSerializer.Serialize(scheduleItems)
        };
        _dbContext.ScheduleItems.Add(entity);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<string?> GetScheduleItems(int offerInstanceId)
        => await _dbContext.ScheduleItems
            .AsNoTracking()
            .Where(t => t.OfferInstanceId == offerInstanceId)
            .Select(t => t.Data)
            .FirstOrDefaultAsync();

    public async Task<Entities.OfferInstance> Get(int offerInstanceId)
        => await _dbContext.OfferModelations
           .AsNoTracking()
           .Where(t => t.OfferInstanceId == offerInstanceId)
           .FirstAsync();
}
