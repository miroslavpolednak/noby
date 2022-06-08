﻿using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api;

internal static class CaseExtensions
{
    //private static readonly string[] EasTaskMandatoryFields = new string[] {"ukol_id", "ukol_sada", "ukol_typ", "ukol_nazov", "ukol_dat_start_proces", "ukol_stav_poz"};

    public static WorkflowTask ToWorkflowTask(this ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper.WFS_FindItem easTask)
    {
        var dict = easTask.task.ToDictionary(i => i.mtdt_def, i => i.mtdt_val);

        //TODO: remove mocks

        var mockTypeId = 1;
        var mockCreatedOn = DateTime.Now;
        var mockStateId = 0;

        var task = new WorkflowTask
        {
            TaskId = int.Parse(dict["ukol_id"]),
            TaskProcessId = int.Parse(dict["ukol_sada"]),
            TypeId = dict.ContainsKey("ukol_typ") ? int.Parse(dict["ukol_typ"]) : mockTypeId,
            Name = dict["ukol_nazov"],
            CreatedOn = dict.ContainsKey("ukol_dat_start_proces") ? DateTime.Parse(dict["ukol_dat_start_proces"]) : mockCreatedOn,
            StateId = dict.ContainsKey("ukol_stav_poz") ? int.Parse(dict["ukol_stav_poz"]) : mockStateId,
        };

        return task;
    }

    public static UpdateTaskItem ToUpdateTaskItem(this WorkflowTask workflowTask)
    {
        return new UpdateTaskItem
        {
            TaskProcessId = workflowTask.TaskProcessId,
            TypeId = workflowTask.TypeId

        };
    }

}