//using FluentValidation;

//namespace DomainServices.OfferService.Api.Validators;

//internal class SimulateBuildingSavingsRequestValidator : AbstractValidator<Dto.SimulateBuildingSavingsMediatrRequest>
//{
//    public SimulateBuildingSavingsRequestValidator()
//    {
//        RuleFor(t => t.Request.InputData.TargetAmount)
//            .InclusiveBetween(20000, 99999999)
//            .WithMessage(t => $"TargetAmount (={t.Request.InputData.TargetAmount}) must be between 2000 and 99999999").WithErrorCode("10001");

//        RuleFor(t => t.Request.InputData.ProductCode)
//            .GreaterThan(0)
//            .WithMessage(t => $"ProductCode (={t.Request.InputData.ProductCode}) must be > 0").WithErrorCode("10003");
        
//        RuleFor(t => t.Request.InputData.ActionCode)
//            .GreaterThanOrEqualTo(0)
//            .WithMessage(t => $"ActionCode (={t.Request.InputData.ActionCode}) must be >= 0").WithErrorCode("10004");

//        RuleFor(t => t.Request.InputData.ClientIsSVJ)
//            .Must((data, svj) => !data.Request.InputData.ClientIsNaturalPerson || !svj)
//            .WithMessage("ClientIsSVJ=True and ClientIsNaturalPerson=True is invalid combination").WithErrorCode("10002");

//        RuleFor(t => t.Request.InputData.IsWithLoan)
//            .Must((data, isWithLoan) => !isWithLoan || data.Request.InputData.LoanActionCode.GetValueOrDefault(0) > 0)
//            .WithMessage(t => $"LoanActionCode (={t.Request.InputData.LoanActionCode}) must be >= 0").WithErrorCode("10006");

//        RuleFor(t => t.Request.ResourceProcessId)
//            .Must((_, resourceProcessId) => Guid.TryParse(resourceProcessId, out Guid g))
//            .WithMessage("ResourceProcessId is missing or is in invalid format").WithErrorCode("10008");
//    }
//}
