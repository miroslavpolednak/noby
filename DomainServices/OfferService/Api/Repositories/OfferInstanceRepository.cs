using CIS.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DomainServices.OfferService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class OfferInstanceRepository
{
    private readonly OfferServiceDbContext _dbContext;

    public OfferInstanceRepository(OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveOffer(Guid resourceProcessId, int ProductInstanceTypeId, object inputs, object outputs)
    {
        var entity = new Entities.OfferInstance
        {
            ResourceProcessId = resourceProcessId,
            ProductInstanceTypeId = ProductInstanceTypeId,
            Outputs = JsonSerializer.Serialize(outputs),
            Inputs = JsonSerializer.Serialize(inputs)
        };

        _dbContext.OfferModelations.Add(entity);

        await _dbContext.SaveChangesAsync();

        return entity.OfferInstanceId;
    }

    public async Task<Entities.OfferInstance> Get(int offerInstanceId)
        => await _dbContext.OfferModelations
           .AsNoTracking()
           .Where(t => t.OfferInstanceId == offerInstanceId)
           .FirstOrDefaultAsync() ?? throw new CisNotFoundException(13000, $"OfferInstance #{offerInstanceId} not found");

    public async Task<bool> AnyOfResourceProcessId(Guid resourceProcessId)
       => await _dbContext.OfferModelations
          .AsNoTracking()
          .Where(t => t.ResourceProcessId == resourceProcessId)
          .AnyAsync();
}

