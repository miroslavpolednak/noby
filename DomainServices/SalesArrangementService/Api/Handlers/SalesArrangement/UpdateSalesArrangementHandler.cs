using _Usr = DomainServices.UserService.Contracts;
using _Case = DomainServices.CaseService.Contracts;
using CIS.Core.Security;
using DomainServices.UserService.Abstraction;
using Microsoft.EntityFrameworkCore;
using DomainServices.CodebookService.Abstraction;
using CIS.Foms.Enums;
using System.ComponentModel.DataAnnotations;
using DomainServices.CaseService.Abstraction;
using CIS.Core;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class UpdateSalesArrangementDataHandler
    : IRequestHandler<Dto.UpdateSalesArrangementMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.SalesArrangements.FirstOrDefaultAsync(t => t.SalesArrangementId == request.Request.SalesArrangementId, cancellation) 
            ?? throw new CisNotFoundException(16000, $"Sales arrangement ID {request.Request.SalesArrangementId} does not exist.");

        // meni se rbcid
        bool riskBusinessCaseIdChanged = !string.IsNullOrEmpty(request.Request.RiskBusinessCaseId) && !request.Request.RiskBusinessCaseId.Equals(entity.RiskBusinessCaseId, StringComparison.OrdinalIgnoreCase);

        entity.ContractNumber = request.Request.ContractNumber;
        entity.EaCode = request.Request.EaCode;
        entity.RiskBusinessCaseId = request.Request.RiskBusinessCaseId;
        
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
                productType.Mandant,
                request.Request.RiskBusinessCaseId);

            bool sbNotified = ServiceCallResult.Resolve(await _sbWebApiClient.CaseStateChanged(sbNotifyModel, cancellation));
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly ICaseServiceAbstraction _caseService;
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly IUserServiceAbstraction _userService;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApiClient;


    public UpdateSalesArrangementDataHandler(
        ICaseServiceAbstraction caseService,
        ICurrentUserAccessor userAccessor,
        ICodebookServiceAbstraction codebookService,
        IUserServiceAbstraction userService,
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
