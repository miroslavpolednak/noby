using CIS.Core.Security;
using DomainServices.UserService.Clients;
using Microsoft.EntityFrameworkCore;
using DomainServices.CodebookService.Clients;
using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangement;

internal sealed class UpdateSalesArrangementHandler
    : IRequestHandler<Contracts.UpdateSalesArrangementRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.UpdateSalesArrangementRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext
            .SalesArrangements
            .FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

        // kontrola na kategorii
        /*if ((await _codebookService.SalesArrangementTypes(cancellation)).First(t => t.Id == entity.SalesArrangementTypeId).SalesArrangementCategory != 2)
            throw new CisValidationException(18013, $"SalesArrangement type not supported");*/

        // kontrola na stav
        if (!_allowedStates.Contains(entity.State))
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.SalesArrangementCantDelete, entity.State);

        // meni se rbcid
        bool riskBusinessCaseIdChanged = !string.IsNullOrEmpty(request.RiskBusinessCaseId) && !request.RiskBusinessCaseId.Equals(entity.RiskBusinessCaseId, StringComparison.OrdinalIgnoreCase);

        entity.ContractNumber = request.ContractNumber;
        entity.RiskBusinessCaseId = request.RiskBusinessCaseId;

        // pokud je zadost NEW, zmenit na InProgress
        if (entity.State == (int)SalesArrangementStates.NewArrangement)
        {
            entity.State = (int)SalesArrangementStates.InProgress;
        }

        await _dbContext.SaveChangesAsync(cancellation);

        // notifikovat SB
        if (riskBusinessCaseIdChanged)
        {
            // case
            var caseInstance = await _caseService.GetCaseDetail(entity.CaseId, cancellation);

            // get current user's login
            string? userLogin = null;
            if (_userAccessor.User?.Id > 0)
                userLogin = (await _userService.GetUser(_userAccessor.User!.Id, cancellation)).UserIdentifiers.FirstOrDefault()?.Identity ?? "anonymous";

            // get case owner
            var ownerInstance = await _userService.GetUser(caseInstance.CaseOwner.UserId, cancellation);
            var productType = (await _codebookService.ProductTypes(cancellation)).First(t => t.Id == caseInstance.Data.ProductTypeId);

            await _caseService.NotifyStarbuild(entity.CaseId, request.RiskBusinessCaseId, cancellation);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private static int[] _allowedStates = new[] { (int)SalesArrangementStates.NewArrangement, (int)SalesArrangementStates.InProgress, (int)SalesArrangementStates.IsSigned };

    private readonly ICaseServiceClient _caseService;
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly ICodebookServiceClient _codebookService;
    private readonly IUserServiceClient _userService;
    private readonly Database.SalesArrangementServiceDbContext _dbContext;

    public UpdateSalesArrangementHandler(
        ICaseServiceClient caseService,
        ICurrentUserAccessor userAccessor,
        ICodebookServiceClient codebookService,
        IUserServiceClient userService,
        Database.SalesArrangementServiceDbContext dbContext)
    {
        _caseService = caseService;
        _userAccessor = userAccessor;
        _codebookService = codebookService;
        _userService = userService;
        _dbContext = dbContext;
    }
}
