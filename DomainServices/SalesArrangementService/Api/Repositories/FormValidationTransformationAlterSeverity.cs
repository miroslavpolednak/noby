namespace DomainServices.SalesArrangementService.Api.Repositories;

internal enum FormValidationTransformationAlterSeverity : byte
{
    LeaveOriginalSeverity = 0,
    AlterToWarning = 1,
    Ignore = 2
}
