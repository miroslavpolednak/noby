using CIS.Core.Security;
using CIS.Infrastructure.Data;
using DomainServices.CaseService.Api.Database;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace DomainServices.CaseService.Tests;

internal static class DatabaseHelpers
{
    internal static CaseServiceDbContext CreateDbContext(ICurrentUserAccessor currentUser, [CallerMemberName] string memberName = "")
    {
        var options = new DbContextOptionsBuilder<CaseServiceDbContext>()
            .UseInMemoryDatabase(databaseName: $"CaseService_{memberName}")
            .Options;
        var aggregate = new BaseDbContextAggregate<CaseServiceDbContext>(options, new CisEntityFrameworkOptions<CaseServiceDbContext>(), currentUser, TimeProvider.System);
        var dbContext = new CaseServiceDbContext(aggregate);
        dbContext.Database.EnsureDeleted();

        // seed
        dbContext.Cases.Add(CreateEntity(1, EnumCaseStates.InProgress));
        dbContext.Cases.Add(CreateEntity(2, EnumCaseStates.InSigning));
        dbContext.Cases.Add(CreateEntity(3, EnumCaseStates.InProgress));
        dbContext.Cases.Add(CreateEntity(4, EnumCaseStates.InProgress));
        dbContext.Cases.Add(CreateEntity(5, EnumCaseStates.InProgress));

        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();

        return dbContext;
    }

    public static Api.Database.Entities.Case CreateEntity(in long caseId, in EnumCaseStates state)
        => CreateEntity(caseId, state, (e) => { });

    public static Api.Database.Entities.Case CreateEntity(in long caseId, in EnumCaseStates state, Action<Api.Database.Entities.Case> seed)
    {
        var entity = new Api.Database.Entities.Case
        {
            CaseId = caseId,
            State = (int)state,
            CreatedUserId = 1,
            CreatedUserName = "Test",
            CreatedTime = DateTime.Now,
            ContractNumber = "123456789",
            TargetAmount = 1000000,
            ProductTypeId = 20001
        };
        seed(entity);
        return entity;
    }
}
