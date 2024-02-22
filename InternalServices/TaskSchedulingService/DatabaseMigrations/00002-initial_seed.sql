INSERT INTO dbo.ScheduleJob (ScheduleJobId,JobName,JobType,Description,IsDisabled) 
VALUES ('f54733b7-cbbd-4834-8be3-2f005da87896', 'DownloadRdmCodebooks', 'CIS.InternalServices.TaskSchedulingService.Api.Jobs.DownloadRdmCodebooks.DownloadRdmCodebooksHandler', N'JobData: [string] where value is codebook name', 0);

INSERT INTO dbo.ScheduleTrigger (ScheduleTriggerId,ScheduleJobId,TriggerName,Cron,JobData,IsDisabled)
VALUES ('2fe9145e-05cd-4e04-98cd-a9dd0e995be6', 'f54733b7-cbbd-4834-8be3-2f005da87896', 'DownloadRdmCodebooks', '0 3 * * *', N'["CB_CmEpProfessionCategory", "CB_StandardMethodOfArrAcceptanceByNPType", "MAP_CB_CmEpProfessionCategory_CB_CmEpProfession"]', 0);

INSERT INTO dbo.ScheduleJob (ScheduleJobId,JobName,JobType,Description,IsDisabled) 
VALUES ('3A774289-134E-45A3-A360-A706880E18AA', 'CancelCaseBySalesArrangementConditions', 'CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelCaseBySalesArrangementConditions.CancelCaseBySalesArrangementConditionsHandler', N'', 0);

INSERT INTO dbo.ScheduleTrigger (ScheduleTriggerId,ScheduleJobId,TriggerName,Cron,IsDisabled)
VALUES ('3B58AAE7-0744-49B3-ACC7-1FD66BBBD87A', '3A774289-134E-45A3-A360-A706880E18AA', 'CancelCaseBySalesArrangementConditions', '0 5 * * *', 0);

INSERT INTO dbo.ScheduleJob (ScheduleJobId,JobName,JobType,Description,IsDisabled) 
VALUES ('A61AA97D-05C4-4D8F-B488-2EE35B5D2A9C', 'Job1', 'CIS.InternalServices.TaskSchedulingService.Api.Jobs.Job1.Job1Handler', N'Test job 1', 0);

INSERT INTO dbo.ScheduleTrigger (ScheduleTriggerId,ScheduleJobId,TriggerName,Cron,IsDisabled)
VALUES ('6149F6D8-5A14-4892-98DD-973FB24891EB', 'A61AA97D-05C4-4D8F-B488-2EE35B5D2A9C', 'Job1', '* * * * *', 0);

INSERT INTO dbo.ScheduleJob (ScheduleJobId,JobName,JobType,Description,IsDisabled) 
VALUES ('6290CA85-DEAE-4299-AB63-BF3973D3859B', 'Job2', 'CIS.InternalServices.TaskSchedulingService.Api.Jobs.Job2.Job2Handler', N'Test job 2', 0);

INSERT INTO dbo.ScheduleTrigger (ScheduleTriggerId,ScheduleJobId,TriggerName,Cron,IsDisabled)
VALUES ('98D0A582-6314-4BB2-95AB-86D511732483', '6290CA85-DEAE-4299-AB63-BF3973D3859B', 'Job2', '15 * * * *', 0);

INSERT INTO dbo.ScheduleJob (ScheduleJobId,JobName,JobType,Description,IsDisabled) 
VALUES ('6A83A9A5-101F-47B9-BBFE-3FEF6F9D9FA9', 'CancelServiceSalesArrangements', 'CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelServiceSalesArrangements.CancelServiceSalesArrangementsHandler', N'CancelServiceSalesArrangements', 0);

INSERT INTO dbo.ScheduleTrigger (ScheduleTriggerId,ScheduleJobId,TriggerName,Cron,IsDisabled)
VALUES ('63EDCE21-A116-4BE5-B553-B0DD90A55587', '6A83A9A5-101F-47B9-BBFE-3FEF6F9D9FA9', 'CancelServiceSalesArrangements', '0 1 * * *', 0);

INSERT INTO dbo.ScheduleJob (ScheduleJobId,JobName,JobType,Description,IsDisabled) 
VALUES ('5A40D597-9E45-4979-8108-B2BED0F0B86F', 'OfferGuaranteeDateToCheck', 'CIS.InternalServices.TaskSchedulingService.Api.Jobs.OfferGuaranteeDateToCheck.OfferGuaranteeDateToCheckHandler', N'OfferGuaranteeDateToCheck', 0);

INSERT INTO dbo.ScheduleTrigger (ScheduleTriggerId,ScheduleJobId,TriggerName,Cron,IsDisabled)
VALUES ('AC88A6B7-B8E0-482A-98F0-468EB31A97D0', '5A40D597-9E45-4979-8108-B2BED0F0B86F', 'OfferGuaranteeDateToCheck', '0 5 * * *', 0);