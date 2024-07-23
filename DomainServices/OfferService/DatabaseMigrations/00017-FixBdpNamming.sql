DROP TABLE IF EXISTS [bdp].[D_CUST_RETENTION_OFFER]
GO
DROP TABLE IF EXISTS [bdp].[D_CUST_RETENTION_ACCOUNT]
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
	[Installment_Amount] [numeric](16, 4) NOT NULL,
	[Interest_Rate] [numeric](16, 4) NOT NULL,
	[Default_Choice_Flag] [int] NOT NULL,
	[Fixation_Period] [int] NOT NULL,
	[Interest_Rate_Valid_To] [date] NOT NULL,
	[Was_Processed_By_Noby] [bit] NOT NULL
) ON [PRIMARY]
GO
GO
ALTER TABLE [bdp].[D_CUST_RETENTION_OFFER] ADD  CONSTRAINT [DF_D_CUST_RETENTION_OFFER_Was_Processed_By_Noby]  DEFAULT ((0)) FOR [Was_Processed_By_Noby]

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
) ON [PRIMARY]
GO