UPDATE dbo.Offer SET CaseId=B.CaseId, SalesArrangementId=B.SalesArrangementId
FROM dbo.Offer A
INNER JOIN SalesArrangementService.dbo.SalesArrangement B ON A.OfferId=B.OfferId;
