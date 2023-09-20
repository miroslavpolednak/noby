UPDATE DocumentDynamicStringFormat
SET StringFormat = 'jsem politicky exponovanou osobou nebo osobou blízkou k této osobě ve smyslu § 4 z.č. 253/2008 Sb., o~některých~opatřeních proti legalizaci výnosů z trestné činnosti a financování terorismu, ve znění pozdějších předpisů,'
WHERE AcroFieldName IN ('JsemNejsem3Odrazka', 'SpoluzadatelJsemNejsem3Odrazka')