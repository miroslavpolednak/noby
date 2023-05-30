INSERT INTO dbo.FlowSwitch (FlowSwitchId, [Name], [Description], DefaultValue) VALUES
	(7, 'IsOfferWithDiscount', N'IC: Sleva ze sazby nebo na poplatku na modelaci', 0),
	(8, 'DoesWflTaskForIPExist', N'IC: Zadán WFL úkol na IC', 0),
	(9, 'IsWflTaskForIPApproved', N'IC: WFL úkol na IC je schválen', 0),
	(10, 'IsWflTaskForIPNotApproved', N'IC: WFL úkol na IC je zamítnut', 0);
GO

INSERT INTO [dbo].[FlowSwitch2Group] (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES
	(2, 7, 1, 1),
	(2, 7, 3, 1),
	(2, 8, 3, 1),
	(2, 9, 3, 1),
	(2, 10, 3, 0);
GO
