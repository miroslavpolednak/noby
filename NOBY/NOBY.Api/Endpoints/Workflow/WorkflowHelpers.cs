using CIS.Core.Security;

namespace NOBY.Api.Endpoints.Workflow;

/// <summary>
/// https://eacloud.ds.kb.cz/webea/index.php?m=1&o=8FF62FA9-3872-408e-A6ED-DAFD0100901D
/// </summary>
internal static class WorkflowHelpers
{
    public static void ValidateTaskManagePermission(
        int? taskTypeId,
        int? signatureTypeId,
        int? phaseTypeId,
        ICurrentUserAccessor currentUserAccessor)
    {
        if (taskTypeId == 6)
        {
            if (!currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_SigningManage))
            {
                throw new CisAuthorizationException("Task manage permission missing #1");
            }
            else if (signatureTypeId == 1
                && phaseTypeId == 2
                && !currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_SigningAttachments))
            {
                throw new CisAuthorizationException("Task manage permission missing #2");
            }
            else if (currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_SigningAttachments)
                && (signatureTypeId != 1 || !_allowedPhaseTypes.Contains(phaseTypeId.GetValueOrDefault())))
            {
                throw new NobyValidationException(90032);
            }
            else if (!currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_SigningAttachments) 
                && phaseTypeId != 1)
            {
                throw new NobyValidationException(90032);
            }
        }
        else if (taskTypeId != 6 && !currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_OtherManage))
        {
            throw new CisAuthorizationException("Task manage permission missing #5");
        }
    }

    private static int[] _allowedPhaseTypes = new int[] { 1, 2 };
}
