-- WorkflowTaskStatesNoby
TRUNCATE TABLE [dbo].[WorkflowTaskStatesNoby];
INSERT INTO [dbo].[WorkflowTaskStatesNoby] ([Id],[Name],[Filter],[Indicator]) VALUES
(1, 'K VYŘÍZENÍ', 'Active', 'Active'),
(2, 'PROVOZNÍ PODPORA', 'Active', 'Active'),
(3, 'ODESLÁNO', 'Active', 'Active'),
(4, 'DOKONČENO', 'Finished', 'OK'),
(5, 'ZRUŠENO', 'Finished', 'Cancelled');
GO

-- WorkflowProcessStatesNoby
TRUNCATE TABLE [dbo].[WorkflowProcessStatesNoby];
INSERT INTO [dbo].[WorkflowProcessStatesNoby] ([Id],[Name],[Indicator]) VALUES
(1, 'DOKONČENÉ', 'OK'),
(2, 'ZRUŠENÉ', 'Cancelled'),
(3, 'PŘÍPRAVA ŽÁDOSTI', 'Active'),
(4, 'ŽÁDOST PŘEDÁNA KE ZPRACOVÁNÍ', 'Active'),
(5, 'PODPIS ZA KLIENTA', 'Active'),
(6, 'ČERPÁNÍ', 'Active'),
(7, 'SPRÁVA/SPLACENÍ', 'Active'),
(8, 'ZPRACOVÁNÍ', 'Active'),
(9, 'PODPIS BANKY', 'Active'),
(10, 'PODPIS KLIENTA', 'Active'),
(11, 'PŘÍPRAVA', 'Active'),
(12, 'PODPIS', 'Active');
GO