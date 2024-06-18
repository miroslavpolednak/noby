INSERT INTO dbo.ScheduleJob (ScheduleJobId,JobName,JobType,[Description],IsDisabled) 
VALUES ('61006140-1864-4601-917F-814FBEA7655A', 'DeleteBinaryDataFromArchiveQueue', 'CIS.InternalServices.TaskSchedulingService.Api.Jobs.DeleteBinaryDataFromArchiveQueue.DeleteBinaryDataFromArchiveQueueHandler', N'Odmazávání binárek souborů již nahraných do archivu', 0);
GO
INSERT INTO dbo.ScheduleTrigger (ScheduleTriggerId,ScheduleJobId,TriggerName,Cron,IsDisabled)
VALUES ('B3B84BCD-48B0-40DE-8F47-4062ABBBC6E7', '61006140-1864-4601-917F-814FBEA7655A', 'DeleteBinaryDataFromArchiveQueue', '0 3 * * *', 0);