using CIS.Core.Security;

namespace NOBY.Api.Endpoints.Workflow;

internal static class WorkflowHelpers
{
    public static void ValidateTaskManagePermission(
        int? taskTypeId, 
        int? signatureTypeId,
        int? phaseTypeId,
        ICurrentUserAccessor currentUserAccessor)
    {
        if (taskTypeId == 6 && !currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_SigningManage))
        {
            throw new CisAuthorizationException("Task manage permission missing");
        }
        else if (taskTypeId == 6 
            && signatureTypeId == 1
            && phaseTypeId == 2
            && !currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_SigningAttachments))
        {
            throw new CisAuthorizationException("Task manage permission missing");
        }
        else if (taskTypeId != 6 && !currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_OtherManage))
        {
            throw new CisAuthorizationException("Task manage permission missing");
        }
    }
}
