INSERT INTO dbo.ScheduleJob (ScheduleJobId,JobName,JobType,Description,IsDisabled) 
VALUES ('F3E08B8C-8ED1-42B6-A2F4-1E4C5C919FAF', 'CancelConfirmedPriceExceptionCases', 'CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelConfirmedPriceExceptionCases.CancelConfirmedPriceExceptionCasesHandler', N'Job na stornování IC 45 dní po schválení', 0);

INSERT INTO dbo.ScheduleTrigger (ScheduleTriggerId,ScheduleJobId,TriggerName,Cron,IsDisabled)
VALUES ('D1547C76-58A6-45BF-943E-10A6E01C1D6D', 'F3E08B8C-8ED1-42B6-A2F4-1E4C5C919FAF', 'CancelConfirmedPriceExceptionCases', '0 4 * * *', 0);
