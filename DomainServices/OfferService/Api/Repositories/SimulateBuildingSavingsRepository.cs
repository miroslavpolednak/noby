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

    /// <summary>
    /// Ulozeni housing savings s uverem
    /// </summary>
    public async Task<int> Save(
        Guid resourceProcessId,
        SimulationTypes simulationType,
        DateTime timestamp, 
        BuildingSavingsInput inputs, 
        BuildingSavingsData? buildingSavingsData, 
        LoanData? loanData, 
        List<ScheduleItem>? scheduleItems)
    {
        var entity = new Entities.OfferInstance
        {
            ResourceProcessId = resourceProcessId,
            InsertTime = timestamp,
            SimulationType = (byte)simulationType,
            OutputBuildingSavings = JsonSerializer.Serialize(buildingSavingsData),
            OutputScheduleItems = JsonSerializer.Serialize(scheduleItems),
            OutputBuildingSavingsLoan = JsonSerializer.Serialize(loanData),
            Inputs = JsonSerializer.Serialize(inputs)
        };

        _dbContext.OfferModelations.Add(entity);

        await _dbContext.SaveChangesAsync();

        return entity.OfferInstanceId;
    }

    public async Task<GetBuildingSavingsScheduleResponse?> GetScheduleItems(int offerInstanceId)
    {
        var entity = await _dbContext.OfferModelations
            .AsNoTracking()
            .Where(t => t.OfferInstanceId == offerInstanceId)
            .Select(t => new
            {
                SimulationType = t.SimulationType,
                InsertTime = t.InsertTime,
                InsertUserId = t.InsertUserId ?? 0,
                ScheduleItems = t.OutputScheduleItems
            })
            .FirstAsync();

        var model = new GetBuildingSavingsScheduleResponse
        {
            OfferInstanceId = offerInstanceId,
            SimulationType = (SimulationTypes)entity.SimulationType,
            InsertStamp = new(entity.InsertUserId, entity.InsertTime.GetValueOrDefault())
            
        };
        if (!string.IsNullOrEmpty(entity.ScheduleItems))
            model.ScheduleItems.AddRange(JsonSerializer.Deserialize<List<ScheduleItem>>(entity.ScheduleItems));

        return model;
    }

    /// <summary>
    /// Get pro housing savings
    /// </summary>
    public async Task<GetBuildingSavingsDataResponse?> Get(int offerInstanceId)
    {
        var entity = await _dbContext.OfferModelations
           .AsNoTracking()
           .Where(t => t.OfferInstanceId == offerInstanceId)
           .Select(t => new
           {
               SimulationType = t.SimulationType,
               InsertTime = t.InsertTime,
               InsertUserId = t.InsertUserId ?? 0,
               Inputs = t.Inputs,
               OutputBuildingSavings = t.OutputBuildingSavings,
               OutputBuildingSavingsLoan = t.OutputBuildingSavingsLoan
           })
           .FirstAsync();

        return new GetBuildingSavingsDataResponse
        {
            OfferInstanceId = offerInstanceId,
            SimulationType = (SimulationTypes)entity.SimulationType,
            InsertStamp = new(entity.InsertUserId, entity.InsertTime.GetValueOrDefault()),
            InputData = JsonSerializer.Deserialize<BuildingSavingsInput>(entity.Inputs ?? ""),
            BuildingSavings = JsonSerializer.Deserialize<BuildingSavingsData>(entity.OutputBuildingSavings ?? ""),
            Loan = string.IsNullOrEmpty(entity.OutputBuildingSavingsLoan) ? null : JsonSerializer.Deserialize<LoanData>(entity.OutputBuildingSavingsLoan)
        };
    }
}
