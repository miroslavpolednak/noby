UPDATE DocumentDynamicStringFormat
SET StringFormat = 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o~některých~opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,'
WHERE AcroFieldName IN ('JsemNejsem3Odrazka', 'SpoluzadatelJsemNejsem3Odrazka')

UPDATE DocumentSpecialDataField
SET StringFormat = 'Beru na vědomí, že Banka se mnou bude komunikovat přednostně elektronicky, a to prostřednictvím schránky v~internetovém bankovnictví KB+ nebo na mnou uvedený kontaktní e-mail nebo kontaktní telefon, u kterých bylo mnou potvrzeno, že slouží ke komunikaci mezi mnou a Bankou, zejména pro zasílání dokumentace a jejích změn a~zasílání hesel a kódů, v ojedinělých případech mi Banka může doručovat i v listinné podobě na moji Kontaktní adresu. Zároveň beru na vědomí, že tyto kontaktní údaje jsou automaticky předvyplněny, a že v případě, že dojde k~jejich změně Bankou akceptovaným způsobem, bude se mnou Banka komunikovat na tyto nové kontaktní údaje.

Dále beru na vědomí, že do schránky v internetovém bankovnictví KB+ nebo na kontaktní e-mail může Banka zasílat jakékoli písemnosti, včetně písemností, které mohou mít za následek zánik smluvního vztahu.

Schránkou je vyhrazený prostor v internetovém bankovnictví KB+, přístupný prostřednictvím mobilní a webové aplikace KB+, určený pro vzájemnou komunikaci. Schránka nebo její část, stejně jako kontaktní e-mail, slouží jako trvalý nosič dat.'
WHERE DocumentId = 4 AND AcroFieldName = 'ProhlaseniZadatele'