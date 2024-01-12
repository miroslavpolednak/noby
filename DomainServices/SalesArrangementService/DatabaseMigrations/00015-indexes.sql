DROP INDEX IF EXISTS [Idx_SalesArrangementId] ON [dbo].[SalesArrangementParameters];
GO
CREATE NONCLUSTERED INDEX [Idx_SalesArrangementId] ON [dbo].[SalesArrangementParameters]
(
	[SalesArrangementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

DROP INDEX IF EXISTS [Idx_CaseId] ON [dbo].[SalesArrangement];
GO
CREATE NONCLUSTERED INDEX [Idx_CaseId] ON [dbo].[SalesArrangement]
(
	[CaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

DROP INDEX IF EXISTS [Idx_OfferId] ON [dbo].[SalesArrangement];
GO
CREATE NONCLUSTERED INDEX [Idx_OfferId] ON [dbo].[SalesArrangement]
(
	[OfferId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

DROP INDEX IF EXISTS [Idx_SalesArrangementId] ON [dbo].[FlowSwitch];
GO
CREATE NONCLUSTERED INDEX [Idx_SalesArrangementId] ON [dbo].[FlowSwitch]
(
	[SalesArrangementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
