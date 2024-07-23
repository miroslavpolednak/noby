IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'bdp')
BEGIN
    EXEC ('CREATE SCHEMA [bdp]')
END
GO
DROP TABLE IF EXISTS [bdp].[D_CUST_RETENTION_OFFER]
GO
DROP TABLE IF EXISTS [bdp].[D_CUST_RETENTION_BATCH]
GO
DROP TABLE IF EXISTS [bdp].[D_CUST_RETENTION_ACCOUNT]
GO
DROP TABLE IF EXISTS [bdp].[Application_Event]
GO
/****** Object:  Table [bdp].[D_CUST_RETENTION_ACCOUNT]    Script Date: 05.04.2024 15:29:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bdp].[D_CUST_RETENTION_ACCOUNT](
	[Batch_Id] [bigint] NOT NULL,
	[Account_Nbr] [char](16) NOT NULL,
	[Individual_Discount] [numeric](16, 4) NULL,
	[Leave_Probability] [numeric](16, 4) NOT NULL,
	[Calculated_Delta] [numeric](16, 4) NOT NULL,
	[Retention_Campaigns] [varchar](255) NULL,
	[Refixation_Campaigns] [varchar](255) NULL,
	[Refixation_Date] [date] NOT NULL,
	[Was_Processed_By_Noby] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [bdp].[D_CUST_RETENTION_BATCH]    Script Date: 05.04.2024 15:29:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bdp].[D_CUST_RETENTION_BATCH](
	[Batch_Id] [bigint] NOT NULL,
	[Load_Status] [varchar](50) NOT NULL,
	[Was_Processed_By_Noby] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [bdp].[D_CUST_RETENTION_OFFER]    Script Date: 05.04.2024 15:29:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bdp].[D_CUST_RETENTION_OFFER](
	[Batch_Id] [bigint] NOT NULL,
	[Account_Nbr] [char](16) NOT NULL,
	[Offer_Type] [varchar](50) NOT NULL,
	[Offer_Date] [date] NULL,
	[Individual_Discount_Repayment] [numeric](16, 4) NULL,
	[Repayment_Amt] [numeric](16, 4) NOT NULL,
	[Interest_Rate] [numeric](16, 4) NOT NULL,
	[Default_Offer] [int] NOT NULL,
	[Fixation_Period] [int] NOT NULL,
	[Interest_Rate_Valid_To] [date] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [bdp].[D_CUST_RETENTION_ACCOUNT] ADD  CONSTRAINT [DF_D_CUST_RETENTION_ACCOUNT_Was_Processed_By_Noby]  DEFAULT ((0)) FOR [Was_Processed_By_Noby]
GO
ALTER TABLE [bdp].[D_CUST_RETENTION_BATCH] ADD  CONSTRAINT [DF_D_CUST_RETENTION_BATCH_Was_Processed_By_Noby]  DEFAULT ((0)) FOR [Was_Processed_By_Noby]
GO
/****** Object:  Table [bdp].[Application_Event]    Script Date: 09.04.2024 10:36:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bdp].[Application_Event](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Account_Nbr] [nchar](16) NOT NULL,
	[Event_Type] [nchar](10) NULL,
	[Event_Value] [nvarchar](10) NULL,
	[Event_Date] [datetime] NULL,
 CONSTRAINT [PK_Application_Event] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
