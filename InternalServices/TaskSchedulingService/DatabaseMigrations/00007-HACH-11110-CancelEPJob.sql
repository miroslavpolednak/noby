INSERT INTO dbo.ScheduleJob (ScheduleJobId,JobName,JobType,[Description],IsDisabled) 
VALUES ('BC905E06-8A9D-48C5-831D-A174F2149B63', 'CancelNotFinishedExtraPayments', 'CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelNotFinishedExtraPayments.CancelNotFinishedExtraPaymentsHandler', N'Automatické stornování nedokončených Mimořádných splátek', 0);
GO
INSERT INTO dbo.ScheduleTrigger (ScheduleTriggerId,ScheduleJobId,TriggerName,Cron,IsDisabled)
VALUES ('FD506905-C888-457F-B6FF-433E38F43C1F', 'BC905E06-8A9D-48C5-831D-A174F2149B63', 'CancelNotFinishedExtraPayments', '0 6 * * *', 0);