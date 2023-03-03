using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateObligation;

internal sealed class CreateObligationRequestValidator
    : AbstractValidator<CreateObligationRequest>
{
    public CreateObligationRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService, Database.HouseholdServiceDbContext dbContext)
    {
        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CustomerOnSAIdIsEmpty);

        RuleFor(t => t)
            .SetValidator(new Validators.ObligationValidator(codebookService));

        // customer nenalezen v DB
        RuleFor(t => t.CustomerOnSAId)
            .MustAsync(async (customerOnSAId, cancellationToken) => await dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == customerOnSAId, cancellationToken))
            .WithErrorCode(ErrorCodeMapper.CustomerOnSANotFound)
            .ThrowCisException(GrpcValidationBehaviorExceptionTypes.CisNotFoundException);

        /*RuleFor(t => t.Request.Correction)
            .ChildRules(v =>
            {
                v.RuleFor(t => t.CorrectionTypeId)
                    .MustAsync(async (t, token) => !t.HasValue || (await codebookService.ObligationCorrectionTypes(token)).Any(x => x.Id == t.Value))
                    .WithMessage("ObligationTypeId is not valid").WithErrorCode("0");
            });*/
    }
}

