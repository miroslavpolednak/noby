--Old (basic info list)
--SELECT Id PartnerId,
--	Jmeno FirstName,
--	Prijmeni LastName,
--	Pohlavi GenderId,
--	RodneCisloIco BirthNumber,
--	DatumNarozeni BirthDate,
--	PrukazTotoznosti IdentificationDocumentNumber,
--	TypDokladu IdentificationDocumentTypeId,
--	PrukazVydalDatum IdentificationDocumentIssuedOn,
--	PrukazVydal IdentificationDocumentIssuedBy,
--	PrukazStatVydaniId IdentificationDocumentIssuingCountryId,
--	PreukazPlatnostDo IdentificationDocumentValidTo
--FROM dbo.PARTNER
--WHERE Id IN @partnerIds
SELECT Id PartnerId,
	Titul DegreeBefore,
	Jmeno FirstName,
	Prijmeni LastName,
	RodneCisloIco BirthNumber,
	DatumNarozeni BirthDate,
	MistoNarozeni PlaceOfBirth,
	Pohlavi GenderId,
	PEP IsPoliticallyExposed,
	StatniPrislusnostId CitizenshipCountryId,
	PrukazTotoznosti IdentificationDocumentNumber,
	TypDokladu IdentificationDocumentTypeId,
	PrukazVydalDatum IdentificationDocumentIssuedOn,
	PrukazVydal IdentificationDocumentIssuedBy,
	PrukazStatVydaniId IdentificationDocumentIssuingCountryId,
	PreukazPlatnostDo IdentificationDocumentValidTo,
	Ulice Street,
	CisloDomu2 StreetNumber,
	CisloDomu1 HouseNumber,
	Psc PostCode,
	Misto City,
	VypisyUlice MailingStreet,
	VypisyCisloDomu2 MailingStreetNumber,
	VypisyCisloDomu1 MailingHouseNumber,
	VypisyPsc MailingPostCode,
	VypisyMisto MailingCity,
	KBId KbId,
	contact.KontaktId ContactId,
	contact.PrimarniKontakt IsPrimaryContact,
	contact.TypKontaktu ContactType,
	contact.[Value]
FROM dbo.PARTNER a
OUTER APPLY (
	SELECT Id AS KontaktId,
		PrimarniKontakt,
		TypKontaktu,
		[Value]
	FROM dbo.PartnerKontakty k
	WHERE k.PartnerId = a.Id
		AND PlatnostOd < GetDate()
		AND (
			PlatnostDo IS NULL
			OR PlatnostDo < GetDate()
			)
	) contact
WHERE Id IN @partnerIds
