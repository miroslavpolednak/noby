/****** Object:  StoredProcedure [dbo].[DeleteRefixationOfferOlderThan]    Script Date: 09.04.2024 10:36:23 ******/
DROP PROCEDURE IF EXISTS [dbo].[DeleteRefixationOfferOlderThan]
GO
/****** Object:  Table [dbo].[Application_Event]    Script Date: 09.04.2024 10:36:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Application_Event]') AND type in (N'U'))
DROP TABLE [dbo].[Application_Event]
GO
/****** Object:  Table [dbo].[Application_Event]    Script Date: 09.04.2024 10:36:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Application_Event](
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
/****** Object:  StoredProcedure [dbo].[DeleteRefixationOfferOlderThan]    Script Date: 09.04.2024 10:36:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create  PROC [dbo].[DeleteRefixationOfferOlderThan] 
  @Date DATE
AS
BEGIN
BEGIN TRANSACTION;
BEGIN TRY
SET NOCOUNT ON;

SELECT 
o.OfferId,
o.Flags
INTO #ForDelete
FROM dbo.Offer o
WHERE o.ValidTo < @Date AND o.OfferType = 2 AND o.Origin = 1 --OfferType 2 > Refixation -- Origin 1 > BigDataPlatform

DELETE o
FROM dbo.Offer o
INNER JOIN #ForDelete d ON d.OfferId = o.OfferId

DELETE r
FROM DDS.MortgageRefixationData r
INNER JOIN #ForDelete d ON d.OfferId = r.DocumentDataEntityId

DROP TABLE #ForDelete

COMMIT TRANSACTION;

END TRY
BEGIN CATCH
    -- If an error occurs, roll back the transaction
    ROLLBACK TRANSACTION;
    -- Handle the error
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(@ErrorMessage, 16, 1);
END CATCH;
END
GO
