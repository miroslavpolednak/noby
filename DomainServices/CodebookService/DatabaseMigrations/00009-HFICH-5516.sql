CREATE TABLE [dbo].[WorkflowConsultationMatrix](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TaskSubtypeId] [int] NOT NULL,
	[ProcessTypeId] [int] NOT NULL,
	[ProcessPhaseId] [int] NOT NULL,
	[IsConsultation] [bit] NOT NULL,
 CONSTRAINT [PK_WorkflowConsultationMatrix] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[WorkflowConsultationMatrix] ON 
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (1, 0, 1, 1, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (2, 0, 1, 2, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (3, 0, 1, 3, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (4, 0, 1, 4, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (5, 0, 1, 5, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (6, 0, 1, 6, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (7, 0, 2, 1, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (8, 0, 2, 2, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (9, 0, 2, 3, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (10, 0, 3, 1, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (11, 0, 3, 2, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (12, 0, 3, 3, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (13, 1, 1, 1, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (14, 1, 1, 2, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (15, 1, 1, 5, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (16, 1, 2, 1, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (17, 1, 2, 2, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (18, 1, 2, 3, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (19, 2, 1, 5, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (20, 3, 1, 1, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (21, 3, 1, 2, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (22, 3, 1, 4, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (23, 3, 1, 5, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (24, 3, 1, 6, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (25, 3, 2, 1, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (26, 3, 2, 2, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (27, 3, 2, 3, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (28, 4, 1, 5, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (29, 4, 1, 6, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (30, 5, 1, 5, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (31, 5, 1, 6, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (32, 5, 2, 1, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (33, 5, 2, 2, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (34, 5, 2, 3, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (35, 5, 3, 1, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (36, 5, 3, 2, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (37, 5, 3, 3, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (38, 6, 1, 5, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (39, 6, 1, 6, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (40, 6, 2, 1, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (41, 6, 2, 2, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (42, 6, 2, 3, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (43, 7, 1, 5, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (44, 8, 1, 1, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (45, 8, 1, 2, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (46, 8, 1, 3, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (47, 8, 1, 4, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (48, 9, 3, 1, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (49, 9, 3, 2, 1)
GO
INSERT [dbo].[WorkflowConsultationMatrix] ([Id], [TaskSubtypeId], [ProcessTypeId], [ProcessPhaseId], [IsConsultation]) VALUES (50, 9, 3, 3, 1)
GO
SET IDENTITY_INSERT [dbo].[WorkflowConsultationMatrix] OFF
GO
ALTER TABLE [dbo].[WorkflowConsultationMatrix] ADD  CONSTRAINT [DF_WorkflowConsultationMatrix_IsConsultation]  DEFAULT ((1)) FOR [IsConsultation]
GO
