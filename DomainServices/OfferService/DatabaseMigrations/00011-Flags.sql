alter table dbo.Offer add Flags int default(0);
go
update dbo.Offer set Flags = 0;
