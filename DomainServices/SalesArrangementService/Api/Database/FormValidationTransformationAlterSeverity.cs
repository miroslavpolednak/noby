namespace DomainServices.SalesArrangementService.Api.Database;

internal enum FormValidationTransformationAlterSeverity : byte
{
    LeaveOriginalSeverity = 0,
    AlterToWarning = 1,
    Ignore = 2
}
