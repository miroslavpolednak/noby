alter table dbo.Offer add OfferType int;
go
update dbo.Offer set OfferType=1;
