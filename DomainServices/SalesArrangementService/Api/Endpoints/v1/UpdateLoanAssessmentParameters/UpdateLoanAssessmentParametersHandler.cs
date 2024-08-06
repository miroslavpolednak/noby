using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.UserService.Clients;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateLoanAssessmentParameters;

internal sealed class UpdateLoanAssessmentParametersHandler(
    SalesArrangementServiceDbContext _dbContext, 
    ICaseServiceClient _caseService, 
    ICurrentUserAccessor _userAccessor, 
    ICodebookServiceClient _codebookService, 
    IUserServiceClient _userService)
		: IRequestHandler<UpdateLoanAssessmentParametersRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateLoanAssessmentParametersRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext
            .SalesArrangements
            .FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);
        
        // puvodni rbcid
        string? rbcid = entity.RiskBusinessCaseId;

        if (!string.IsNullOrEmpty(request.RiskSegment))
        {
            entity.RiskSegment = request.RiskSegment;
        }

        if (!string.IsNullOrEmpty(request.CommandId))
        {
            entity.CommandId = request.CommandId;
        }

        if (!string.IsNullOrEmpty(request.LoanApplicationAssessmentId))
        {
            entity.LoanApplicationAssessmentId = request.LoanApplicationAssessmentId;
        }

        if (!string.IsNullOrEmpty(request.RiskBusinessCaseId))
        {
            entity.RiskBusinessCaseId = request.RiskBusinessCaseId;
        }

        if (request.RiskBusinessCaseExpirationDate is not null)
        {
            entity.FirstLoanAssessmentDate ??= DateTime.Now;
            entity.RiskBusinessCaseExpirationDate = request.RiskBusinessCaseExpirationDate;
        }

        if (!string.IsNullOrEmpty(request.LoanApplicationDataVersion))
        {
            entity.LoanApplicationDataVersion = request.LoanApplicationDataVersion;
        }

        // pokud je zadost NEW, zmenit na InProgress
        if (entity.State == (int)EnumSalesArrangementStates.NewArrangement)
        {
            entity.State = (int)EnumSalesArrangementStates.InProgress;
        }

        // ulozeni zmen
        await _dbContext.SaveChangesAsync(cancellation);

        // meni se rbcid, notifikovat SB
        if (!string.IsNullOrEmpty(request.RiskBusinessCaseId) && !request.RiskBusinessCaseId.Equals(rbcid, StringComparison.OrdinalIgnoreCase))
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
}
