using _Usr = DomainServices.UserService.Contracts;
using _Case = DomainServices.CaseService.Contracts;
using CIS.Core.Security;
using DomainServices.UserService.Clients;
using Microsoft.EntityFrameworkCore;
using DomainServices.CodebookService.Abstraction;
using CIS.Foms.Enums;
using System.ComponentModel.DataAnnotations;
using DomainServices.CaseService.Clients;
using CIS.Core;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class UpdateSalesArrangementHandler
    : IRequestHandler<Dto.UpdateSalesArrangementMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.SalesArrangements.FirstOrDefaultAsync(t => t.SalesArrangementId == request.Request.SalesArrangementId, cancellation) 
            ?? throw new CisNotFoundException(18000, $"Sales arrangement ID {request.Request.SalesArrangementId} does not exist.");

        // kontrola na kategorii
        /*if ((await _codebookService.SalesArrangementTypes(cancellation)).First(t => t.Id == entity.SalesArrangementTypeId).SalesArrangementCategory != 2)
            throw new CisValidationException(18013, $"SalesArrangement type not supported");*/

        // kontrola na stav
        if (entity.State != (int)SalesArrangementStates.InProgress && entity.State != (int)SalesArrangementStates.IsSigned)
            throw new CisValidationException(18082, $"SalesArrangement cannot be updated/deleted in this state {entity.State}");

        // meni se rbcid
        bool riskBusinessCaseIdChanged = !string.IsNullOrEmpty(request.Request.RiskBusinessCaseId) && !request.Request.RiskBusinessCaseId.Equals(entity.RiskBusinessCaseId, StringComparison.OrdinalIgnoreCase);

        entity.ContractNumber = request.Request.ContractNumber;
        entity.RiskBusinessCaseId = request.Request.RiskBusinessCaseId;
        entity.FirstSignedDate = request.Request.FirstSignedDate;
        entity.SalesArrangementSignatureTypeId = request.Request.SalesArrangementSignatureTypeId;

        await _dbContext.SaveChangesAsync(cancellation);

        // notifikovat SB
        if (riskBusinessCaseIdChanged)
        {
            // case
            var caseInstance = ServiceCallResult.ResolveAndThrowIfError<_Case.Case>(await _caseService.GetCaseDetail(entity.CaseId, cancellation));

            // get current user's login
            string? userLogin = null;
            if (_userAccessor.User?.Id > 0)
                userLogin = ServiceCallResult.ResolveAndThrowIfError<_Usr.User>(await _userService.GetUser(_userAccessor.User!.Id, cancellation)).UserIdentifiers.First().Identity;

            // get case owner
            var ownerInstance = ServiceCallResult.ResolveAndThrowIfError<_Usr.User>(await _userService.GetUser(caseInstance.CaseOwner.UserId, cancellation));
            var productType = (await _codebookService.ProductTypes(cancellation)).First(t => t.Id == caseInstance.Data.ProductTypeId);

            var sbNotifyModel = new ExternalServices.SbWebApi.Shared.CaseStateChangedModel(
                userLogin ?? "anonymous",
                entity.CaseId,
                entity.ContractNumber ?? "",
                $"{caseInstance.Customer?.FirstNameNaturalPerson} {caseInstance.Customer?.Name}",
                ((CaseStates)caseInstance.State).GetAttribute<DisplayAttribute>()!.Name!,
                caseInstance.Data.ProductTypeId,
                ownerInstance.CPM,
                ownerInstance.ICP,
                (Mandants)productType.MandantId,
                request.Request.RiskBusinessCaseId);

            bool sbNotified = ServiceCallResult.Resolve(await _sbWebApiClient.CaseStateChanged(sbNotifyModel, cancellation));
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly ICaseServiceClient _caseService;
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly IUserServiceClient _userService;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApiClient;


    public UpdateSalesArrangementHandler(
        ICaseServiceClient caseService,
        ICurrentUserAccessor userAccessor,
        ICodebookServiceAbstraction codebookService,
        IUserServiceClient userService,
        Repositories.SalesArrangementServiceDbContext dbContext,
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
