using SharedTypes.Enums;
using DomainServices.DocumentOnSAService.Contracts;
using FastEnumUtility;
using FluentValidation;
using Microsoft.FeatureManagement;
using DomainServices.CodebookService.Clients;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StartSigning;

public class StartSigningValidator : AbstractValidator<StartSigningRequest>
{
    public StartSigningValidator(IFeatureManager featureManager, ICodebookServiceClient codebookService)
    {
        RuleFor(e => e.SalesArrangementId).NotNull().WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsRequired);

        RuleFor(d => d)
            .Must(ValidateDocumentType)
            .WithErrorCode(ErrorCodeMapper.DocumentTypeIdDoesNotExist);

        RuleFor(t => t.SignatureTypeId)
            .MustAsync(async (t, _) => t != (int)SignatureTypes.Electronic || await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.ElectronicSigning))
            .WithErrorCode(ErrorCodeMapper.ElectronicSigningFeatureIsDisabled);

        RuleFor(t => t)
            .MustAsync(async (request, _) => request.SignatureTypeId != (int)SignatureTypes.Electronic || request.TaskId is null || await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.ElectronicWorkflowSigning))
            .WithErrorCode(ErrorCodeMapper.ElectronicSigningFeatureIsDisabled);

        RuleFor(t => t)
          .MustAsync(async (request, ct) => request.SignatureTypeId != (int)SignatureTypes.Electronic || request.TaskId is not null || (await codebookService.DocumentTypes(ct)).Single(d => d.Id == request.DocumentTypeId).IsElectronicSigningEnabled)
          .WithErrorCode(ErrorCodeMapper.ElectronicSigningFeatureIsDisabled);
    }

    private readonly Func<StartSigningRequest, bool> ValidateDocumentType = (request) =>
    {
        // if not worflow 
        if (request.TaskId is null)
        {
            if (request.DocumentTypeId is null)
                return false;

            if (!FastEnum.IsDefined((DocumentTypes)request.DocumentTypeId.Value))
                return false;
        }
        return true;
    };

}

public static class StartSigningBlValidator
{
    public static void ValidateProductRequest(StartSigningRequest request)
    {
        ValidateSignatureType(request);
        ValidateCustomerOnSa(request);
    }

    public static void ValidateCrsRequest(StartSigningRequest request)
    {
        ValidateSignatureType(request);

        if (request.CustomerOnSAId1 is null)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CustomerOnSAIdRequired);
    }

    public static void ValidateWorkflowRequest(StartSigningRequest request)
    {
        if (request.CaseId is null)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.WorkflowRequestCaseIdRequired);
    }

    public static void ValidateServiceRequest(StartSigningRequest request)
    {
        ValidateSignatureType(request);
    }

    private static void ValidateCustomerOnSa(StartSigningRequest request)
    {
        if (request.CustomerOnSAId1 is null && request.CustomerOnSAId2 is null)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CustomerOnSAIdRequired);
    }

    private static void ValidateSignatureType(StartSigningRequest request)
    {
        if (request.SignatureTypeId != (int)SignatureTypes.Paper && request.SignatureTypeId != (int)SignatureTypes.Electronic)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.OnlyElectronicOrPaperSignatureSupported);
    }
}