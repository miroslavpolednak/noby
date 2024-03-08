using CIS.InternalServices.TaskSchedulingService.Contracts;
using FluentValidation;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.GetJobStatuses;

internal sealed class GetJobStatusesRequestValidator
    : AbstractValidator<GetJobStatusesRequest>
{
    public GetJobStatusesRequestValidator()
    {
        RuleFor(t => t.PageSize)
            .GreaterThan(0);

        RuleFor(t => t.Page)
            .GreaterThan(0);
    }
}
