﻿using CIS.Core.Security;

namespace NOBY.Api.Endpoints.Workflow;

/// <summary>
/// https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=8FF62FA9-3872-408e-A6ED-DAFD0100901D
/// </summary>
internal static class WorkflowHelpers
{
    public static void ValidateTaskManagePermission(
        in int? taskTypeId,
        in int? signatureTypeId,
        in int? phaseTypeId,
        in int? processTypeId,
        ICurrentUserAccessor currentUserAccessor)
    {
        if (taskTypeId == 6)
        {
            ValidateRefinancing(processTypeId, currentUserAccessor, UserPermissions.WFL_TASK_DETAIL_SigningManage, UserPermissions.WFL_TASK_DETAIL_RefinancingSigningManage);

            if (signatureTypeId == 1
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
        else
        {
            ValidateRefinancing(processTypeId, currentUserAccessor, UserPermissions.WFL_TASK_DETAIL_OtherManage, UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage);
        }
    }

    public static void ValidateRefinancing(in int? processTypeId, ICurrentUserAccessor currentUserAccessor, in UserPermissions inPermission, in UserPermissions notInPermission)
    {
        if (processTypeId.HasValue)
        {
            if (processTypeId is ((int)WorkflowProcesses.Main or (int)WorkflowProcesses.Change) && !currentUserAccessor.HasPermission(inPermission))
            {
                throw new CisAuthorizationException($"Task manage authorization failed. ProcessTypeId is {processTypeId} but missing permission {inPermission}");
            }

            if (processTypeId is not ((int)WorkflowProcesses.Main or (int)WorkflowProcesses.Change) && !currentUserAccessor.HasPermission(notInPermission))
            {
                throw new CisAuthorizationException($"Task manage authorization failed. ProcessTypeId is {processTypeId} but missing permission {notInPermission}");
            }
        }
    }

    private static int[] _allowedPhaseTypes = [ 1, 2 ];
}
