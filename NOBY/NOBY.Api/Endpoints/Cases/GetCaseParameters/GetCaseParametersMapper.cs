using CIS.Core.Attributes;
using DomainServices.CodebookService.Contracts;

namespace NOBY.Api.Endpoints.Cases.GetCaseParameters;

[SelfService, ScopedService]
internal sealed class GetCaseParametersMapper(DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService)
{
    public async Task<CasesGetCaseParametersCodebookItem?> GetProductType(int productTypeId, CancellationToken cancellationToken)
    {
        var productTypes = await _codebookService.ProductTypes(cancellationToken);
        var instance = productTypes.FirstOrDefault(t => t.Id == productTypeId);

        return instance is null ? null : getCodebookItem(instance);
    }

    public async Task<ShareTypesGenericCodebookItem?> GetLoanKind(int loanKindId, CancellationToken cancellationToken)
    {
        var loanKinds = await _codebookService.LoanKinds(cancellationToken);
        var instance = loanKinds.FirstOrDefault(t => t.Id == loanKindId);

        return instance is null ? null : new ShareTypesGenericCodebookItem
        {
            Id = instance.Id,
            Name = instance.Name,
            IsValid = instance.IsValid,
            Code = instance.Code,
            Description = instance.Description,
            MandantId = instance.MandantId,
            IsDefault = instance.IsDefault
        };
    }

    public async Task<List<CasesGetCaseParametersLoanPurposeItem>?> GetLoanPurposes(IEnumerable<DomainServices.OfferService.Contracts.LoanPurpose> loanPurposes, CancellationToken cancellationToken)
    {
        var loanPurposeTypes = await _codebookService.LoanPurposes(cancellationToken);

        return loanPurposes.Select(l =>
        {
            var purposeType = loanPurposeTypes.First(t => t.Id == l.LoanPurposeId);

            return new CasesGetCaseParametersLoanPurposeItem
            {
                LoanPurpose = getCodebookItem(purposeType),
                Sum = l.Sum
            };
        }).ToList();
    }

    public async Task<List<CasesGetCaseParametersLoanPurposeItem>?> GetLoanPurposes(IEnumerable<DomainServices.ProductService.Contracts.LoanPurpose> loanPurposes, CancellationToken cancellationToken)
    {
        var loanPurposeTypes = await _codebookService.LoanPurposes(cancellationToken);

        return loanPurposes.Select(l =>
        {
            var purposeType = loanPurposeTypes.First(t => t.Id == l.LoanPurposeId);

            return new CasesGetCaseParametersLoanPurposeItem
            {
                LoanPurpose = getCodebookItem(purposeType),
                Sum = l.Sum
            };
        }).ToList();
    }

#pragma warning disable CA1822 // Mark members as static
    public CasesGetCaseParametersCaseOwnerUser? GetCaseOwnerOrigUser(
#pragma warning restore CA1822 // Mark members as static
        DomainServices.UserService.Clients.Dto.UserDto? caseOwnerOrig,
        DomainServices.UserService.Clients.Dto.UserDto? caseOwnerCurrent)
    {
        var user = caseOwnerOrig ?? caseOwnerCurrent;

        if (user is null) return null;

        var identifiers = user?.UserIdentifiers ?? Enumerable.Empty<SharedTypes.GrpcTypes.UserIdentity>();

        return new CasesGetCaseParametersCaseOwnerUser
        {
            BranchName = user?.UserInfo.PersonOrgUnitName ?? user?.UserInfo.DealerCompanyName,
            ConsultantName = user?.UserInfo.DisplayName,
            Cpm = user?.UserInfo.Cpm,
            Icp = user?.UserInfo.Icp,
            IsInternal = user?.UserInfo.IsInternal ?? false,
            UserIdentifiers = identifiers.Select(i => new SharedTypesUserIdentity
            {
                Scheme = (SharedTypesUserIdentityScheme)i.IdentityScheme,
                Identity = i.Identity
            }).ToList()
        };
    }

    private static CasesGetCaseParametersCodebookItem getCodebookItem(IBaseCodebook item)
    {
        return new CasesGetCaseParametersCodebookItem()
        {
            Id = item.Id,
            Name = item.Name,
            IsValid = item.IsValid
        };
    }
}
