using DomainServices.CodebookService.Clients;
using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.CreateMortgageResponseCode;

internal sealed class CreateMortgageResponseCodeRequestValidator
    : AbstractValidator<RefinancingCreateMortgageResponseCodeRequest>
{
    public CreateMortgageResponseCodeRequestValidator(ICodebookServiceClient codebookService, IFeatureManager featureManager)
    {
        RuleFor(t => t.ResponseCodeTypeId)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (id, c) => (await codebookService.ResponseCodeTypes(c)).Any(t => t.Id == id))
            .WithMessage("Unknown ResponseCodeTypeId")
            .MustAsync(async (req, id, c) =>
            {
                var cb = (await codebookService.ResponseCodeTypes(c)).First(t => t.Id == id);
                return cb.DataType switch
                {
                    DomainServices.CodebookService.Contracts.v1.ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.Date => req.DataDateTime.HasValue,
                    DomainServices.CodebookService.Contracts.v1.ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.BankCode => (await codebookService.BankCodes(c)).Any(t => t.BankCode == req.DataBankCode),
                    _ => true
                };
            })
            .WithMessage("Unknown ResponseCodeTypeId")
            .MustAsync(async (req, id, c) =>
            {
                var cb = (await codebookService.ResponseCodeTypes(c)).First(t => t.Id == id);
                if (cb.IsAvailableForRetention && await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Retention))
                {
                    return true;
                }
                else if (cb.IsAvailableForRefixation && await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Refixation))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            })
            .WithErrorCode(90032);
    }
}
