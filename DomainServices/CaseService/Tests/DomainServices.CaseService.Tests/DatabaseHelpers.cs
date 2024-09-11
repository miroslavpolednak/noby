using CIS.Core.Security;
using CIS.Infrastructure.Data;
using DomainServices.CaseService.Api.Database;
using Microsoft.EntityFrameworkCore;
using SharedTypes.Enums;
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
        dbContext.Cases.Add(createEntity(1, EnumCaseStates.InProgress));
        dbContext.Cases.Add(createEntity(2, EnumCaseStates.InSigning));
        dbContext.Cases.Add(createEntity(3, EnumCaseStates.InProgress));
        dbContext.Cases.Add(createEntity(4, EnumCaseStates.InProgress));
        dbContext.Cases.Add(createEntity(5, EnumCaseStates.InProgress));

        dbContext.SaveChanges();

        return dbContext;
    }

    private static Api.Database.Entities.Case createEntity(in long caseId, in EnumCaseStates state)
        => createEntity(caseId, state, (e) => { });

    private static Api.Database.Entities.Case createEntity(in long caseId, in EnumCaseStates state, Action<Api.Database.Entities.Case> seed)
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
