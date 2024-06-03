--Case
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'NOBY_Case') AND type in (N'SN'))
	DROP SYNONYM [dbo].[NOBY_Case]

CREATE SYNONYM [dbo].[NOBY_Case_S] FOR [ADPRA175].[CaseService].[dbo].[Case]
CREATE SYNONYM [dbo].[NOBY_CaseHistory_S] FOR [ADPRA175].[CaseService].[dbo].[CaseHistory]

--Offer
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOBY_Offer]') AND type in (N'SN'))
	DROP SYNONYM [dbo].[NOBY_Offer]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOBY_OfferData]') AND type in (N'SN'))
	DROP SYNONYM [dbo].[NOBY_OfferData]

CREATE SYNONYM [dbo].[NOBY_Offer_S] FOR [ADPRA175].[OfferService].[dbo].[Offer]
CREATE SYNONYM [dbo].[NOBY_OfferMortgageData_S] FOR [ADPRA175].[OfferService].[DDS].[MortgageOfferData]

--SA
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOBY_SalesArrangement]') AND type in (N'SN'))
	DROP SYNONYM [dbo].[NOBY_SalesArrangement]

CREATE SYNONYM [dbo].[NOBY_SalesArrangement_S] FOR [ADPRA175].[SalesArrangementService].[dbo].[SalesArrangement]
CREATE SYNONYM [dbo].[NOBY_SalesArrangementHistory_S] FOR [ADPRA175].[SalesArrangementService].[dbo].[SalesArrangementHistory]