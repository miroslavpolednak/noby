update [DDS].[RealEstateValuationOrderData] set DocumentDataEntityId=b.RealEstateValuationId
from [DDS].[RealEstateValuationOrderData] a inner join dbo.RealEstateValuation b on a.DocumentDataEntityId=b.OrderId
where b.OrderId is not null
