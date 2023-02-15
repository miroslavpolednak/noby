using CIS.Core.Security;
using DomainServices.UserService.Clients;
using Microsoft.EntityFrameworkCore;
using DomainServices.CodebookService.Clients;
using CIS.Foms.Enums;
using System.ComponentModel.DataAnnotations;
using DomainServices.CaseService.Clients;
using CIS.Core;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangement;

internal sealed class UpdateSalesArrangementHandler
    : IRequestHandler<Contracts.UpdateSalesArrangementRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.UpdateSalesArrangementRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.SalesArrangements.FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)
            ?? throw new CisNotFoundException(18000, $"Sales arrangement ID {request.SalesArrangementId} does not exist.");

        // kontrola na kategorii
        /*if ((await _codebookService.SalesArrangementTypes(cancellation)).First(t => t.Id == entity.SalesArrangementTypeId).SalesArrangementCategory != 2)
            throw new CisValidationException(18013, $"SalesArrangement type not supported");*/

        // kontrola na stav
        if (entity.State != (int)SalesArrangementStates.InProgress && entity.State != (int)SalesArrangementStates.IsSigned)
            throw new CisValidationException(18082, $"SalesArrangement cannot be updated/deleted in this state {entity.State}");

        // meni se rbcid
        bool riskBusinessCaseIdChanged = !string.IsNullOrEmpty(request.RiskBusinessCaseId) && !request.RiskBusinessCaseId.Equals(entity.RiskBusinessCaseId, StringComparison.OrdinalIgnoreCase);

        entity.ContractNumber = request.ContractNumber;
        entity.RiskBusinessCaseId = request.RiskBusinessCaseId;
        entity.FirstSignedDate = request.FirstSignedDate;
        entity.SalesArrangementSignatureTypeId = request.SalesArrangementSignatureTypeId;

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

            var sbNotifyModel = new ExternalServices.SbWebApi.Dto.CaseStateChangedRequest
            {
                Login = userLogin ?? "anonymous",
                CaseId = entity.CaseId,
                ContractNumber = entity.ContractNumber ?? "",
                ClientFullName = $"{caseInstance.Customer?.FirstNameNaturalPerson} {caseInstance.Customer?.Name}",
                CaseStateName = ((CaseStates)caseInstance.State).GetAttribute<DisplayAttribute>()!.Name!,
                ProductTypeId = caseInstance.Data.ProductTypeId,
                OwnerUserCpm = ownerInstance.CPM,
                OwnerUserIcp = ownerInstance.ICP,
                Mandant = (Mandants)productType.MandantId,
                RiskBusinessCaseId = request.RiskBusinessCaseId,
                IsEmployeeBonusRequested = caseInstance.Data.IsEmployeeBonusRequested
            };
            await _sbWebApiClient.CaseStateChanged(sbNotifyModel, cancellation);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly ICaseServiceClient _caseService;
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly ICodebookServiceClients _codebookService;
    private readonly IUserServiceClient _userService;
    private readonly Database.SalesArrangementServiceDbContext _dbContext;
    private readonly ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApiClient;


    public UpdateSalesArrangementHandler(
        ICaseServiceClient caseService,
        ICurrentUserAccessor userAccessor,
        ICodebookServiceClients codebookService,
        IUserServiceClient userService,
        Database.SalesArrangementServiceDbContext dbContext,
        ExternalServices.SbWebApi.V1.ISbWebApiClient sbWebApiClient)
    {
        _caseService = caseService;
        _userAccessor = userAccessor;
        _codebookService = codebookService;
        _userService = userService;
        _dbContext = dbContext;
        _sbWebApiClient = sbWebApiClient;
    }
}
