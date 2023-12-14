DROP INDEX IF EXISTS [Idx_SalesArrangementId] ON [dbo].[Household];
GO
CREATE NONCLUSTERED INDEX [Idx_SalesArrangementId] ON [dbo].[Household]
(
	[SalesArrangementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

DROP INDEX IF EXISTS [Idx_CustomerOnSAId1] ON [dbo].[Household];
GO
CREATE NONCLUSTERED INDEX [Idx_CustomerOnSAId1] ON [dbo].[Household]
(
	[CustomerOnSAId1] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

DROP INDEX IF EXISTS [Idx_CustomerOnSAId2] ON [dbo].[Household];
GO
CREATE NONCLUSTERED INDEX [Idx_CustomerOnSAId2] ON [dbo].[Household]
(
	[CustomerOnSAId2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

DROP INDEX IF EXISTS [Idx_SalesArrangementId] ON [dbo].[CustomerOnSA];
GO
CREATE NONCLUSTERED INDEX [Idx_SalesArrangementId] ON [dbo].[CustomerOnSA]
(
	[SalesArrangementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

DROP INDEX IF EXISTS [Idx_Identity] ON [dbo].[CustomerOnSAIdentity];
GO
CREATE NONCLUSTERED INDEX [Idx_Identity] ON [dbo].[CustomerOnSAIdentity]
(
	[IdentityScheme] ASC,
	[IdentityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

DROP INDEX IF EXISTS [Idx_CustomerOnSAId] ON [dbo].[CustomerOnSAIdentity];
GO
CREATE NONCLUSTERED INDEX [Idx_CustomerOnSAId] ON [dbo].[CustomerOnSAIdentity]
(
	[CustomerOnSAId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO