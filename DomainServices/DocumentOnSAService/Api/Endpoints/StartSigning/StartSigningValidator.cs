using CIS.Foms.Enums;
using DomainServices.DocumentOnSAService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StartSigning;

public class StartSigningValidator : AbstractValidator<StartSigningRequest>
{
    public StartSigningValidator()
    {
        RuleFor(e => e.SalesArrangementId).NotNull().WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsRequired);
    }
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