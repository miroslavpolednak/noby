using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangement;

internal sealed class UpdateSalesArrangementRequestValidator
    : AbstractValidator<Contracts.UpdateSalesArrangementRequest>
{
    public UpdateSalesArrangementRequestValidator(CodebookService.Clients.ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsEmpty);
    }
}
