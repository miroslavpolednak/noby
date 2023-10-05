TRUNCATE TABLE [dbo].[WorkflowConsultationMatrix];
GO
INSERT INTO [dbo].[WorkflowConsultationMatrix] (TaskSubtypeId, ProcessTypeId, ProcessPhaseId, IsConsultation) VALUES
	(0,1,1,1),
	(0,1,2,1),
	(0,1,3,1),
	(0,1,4,1),
	(0,1,5,1),
	(0,2,1,1),
	(0,2,2,1),
	(0,2,3,1),
	(0,3,1,1),
	(0,3,2,1),
	(0,3,3,1),
	(1,1,1,1),
	(1,1,2,1),
	(1,1,4,1),
	(1,2,1,1),
	(1,2,2,1),
	(1,2,3,1),
	(2,1,4,1),
	(3,1,1,1),
	(3,1,2,1),
	(3,1,3,1),
	(3,1,4,1),
	(3,1,5,1),
	(3,2,1,1),
	(3,2,2,1),
	(3,2,3,1),
	(4,1,4,1),
	(4,1,5,1),
	(5,1,4,1),
	(5,1,5,1),
	(5,2,1,1),
	(5,2,2,1),
	(5,2,3,1),
	(5,3,1,1),
	(5,3,2,1),
	(5,3,3,1),
	(6,1,4,1),
	(6,1,5,1),
	(6,2,1,1),
	(6,2,2,1),
	(6,2,3,1),
	(7,1,4,1),
	(8,1,1,1),
	(8,1,2,1),
	(8,1,3,1),
	(9,3,1,1),
	(9,3,2,1),
	(9,3,3,1);
