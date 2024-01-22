ALTER TABLE [DDS].[RealEstateValuationOrderData] SET ( SYSTEM_VERSIONING = OFF  )
GO

update [DDS].[RealEstateValuationOrderData] set [Data]=replace([Data],'"Standard":null,', '') where [Data] like '%"Standard":null,%';
update [DDS].[RealEstateValuationOrderData] set [Data]=replace([Data],',"Standard":null', '') where [Data] like '%,"Standard":null%';
update [DDS].[RealEstateValuationOrderData] set [Data]=replace([Data],'"OnlinePreorder":null,', '') where [Data] like '%"OnlinePreorder":null,%';
update [DDS].[RealEstateValuationOrderData] set [Data]=replace([Data],',"OnlinePreorder":null', '') where [Data] like '%,"OnlinePreorder":null%';
GO

update [DDS].[RealEstateValudationData] set [Data] = left(A.[Data], len(A.[Data])-1) + (case when len(A.[Data]) > 2 then ',' else '' end) + '"LocalSurveyData":' + B.NewData + '}'
from [DDS].[RealEstateValudationData] A
inner join (
	select [DocumentDataEntityId], data, substring([Data], 58, len([data]) - 58) 'NewData'
	from [RealEstateValuationService].[DDS].[RealEstateValuationOrderData]
	where [Data] like '{"Version":1,"RealEstateValuationOrderType":2,%'
) B on A.[DocumentDataEntityId]=B.[DocumentDataEntityId];

update [DDS].[RealEstateValudationData] set [Data] = left(A.[Data], len(A.[Data])-1) + (case when len(A.[Data]) > 2 then ',' else '' end) + '"LocalSurveyData":' + B.NewData + '}'
from [DDS].[RealEstateValudationData] A
inner join (
	select [DocumentDataEntityId], data, substring([Data], 46, len([data]) - 46) 'NewData'
	from [RealEstateValuationService].[DDS].[RealEstateValuationOrderData]
	where [Data] like '{"RealEstateValuationOrderType":2,%'
) B on A.[DocumentDataEntityId]=B.[DocumentDataEntityId];

update [DDS].[RealEstateValudationData] set [Data] = left(A.[Data], len(A.[Data])-1) + (case when len(A.[Data]) > 2 then ',' else '' end) + '"OnlinePreorderDetails":' + B.NewData + '}'
from [DDS].[RealEstateValudationData] A
inner join (
	select [DocumentDataEntityId], data, substring([Data], 64, len([data]) - 64) 'NewData'
	from [RealEstateValuationService].[DDS].[RealEstateValuationOrderData]
	where [Data] like '{"Version":1,"RealEstateValuationOrderType":1,%'
) B on A.[DocumentDataEntityId]=B.[DocumentDataEntityId];

update [DDS].[RealEstateValudationData] set [Data] = left(A.[Data], len(A.[Data])-1) + (case when len(A.[Data]) > 2 then ',' else '' end) + '"OnlinePreorderDetails":' + B.NewData + '}'
from [DDS].[RealEstateValudationData] A
inner join (
	select [DocumentDataEntityId], data, substring([Data], 52, len([data]) - 52) 'NewData'
	from [RealEstateValuationService].[DDS].[RealEstateValuationOrderData]
	where [Data] like '{"RealEstateValuationOrderType":1,%'
) B on A.[DocumentDataEntityId]=B.[DocumentDataEntityId];

