  UPDATE s
  SET 
  s.JobName = 'CancelSelectedPriceExceptionCases', 
  s.Description='Automatické stornování schválených IC ke všem druhům procesů a zamítnutých IC k Retenčnímu procesu 45 dní po schválení/zamítnutí',
  s.JobType = 'CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelSelectedPriceExceptionCases.CancelSelectedPriceExceptionCasesHandler'
  FROM dbo.ScheduleJob s
  WHERE s.ScheduleJobId = 'F3E08B8C-8ED1-42B6-A2F4-1E4C5C919FAF'
  GO
  UPDATE t
  SET t.TriggerName = 'CancelSelectedPriceExceptionCases'
  FROM dbo.ScheduleTrigger t
  WHERE t.ScheduleJobId = 'F3E08B8C-8ED1-42B6-A2F4-1E4C5C919FAF'