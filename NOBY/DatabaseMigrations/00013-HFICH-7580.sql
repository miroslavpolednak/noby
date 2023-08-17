INSERT INTO dbo.FlowSwitch (FlowSwitchId, [Name], [Description], DefaultValue)
	VALUES (14, 'ScoringPerformedAtleastOnce', N'Došlo ke skórování', 0);
GO

INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value])
	VALUES (6, 14, 3, 1);
GO
