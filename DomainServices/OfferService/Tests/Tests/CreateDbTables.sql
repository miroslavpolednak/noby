CREATE TABLE [OfferInstance](
	[OfferInstanceId] INTEGER PRIMARY KEY AUTOINCREMENT,
	[SimulationType] [tinyint] NOT NULL,
	[Inputs] text NOT NULL,
	[OutputBuildingSavings] text NOT NULL,
	[OutputBuildingSavingsLoan] text NULL,
	[OutputScheduleItems] text NULL,
	[InsertUserId] INTEGER NOT NULL,
	[InsertTime] [datetime] NOT NULL
);
