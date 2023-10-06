INSERT INTO DocumentDynamicInputParameter VALUES (9, '001', 2)

INSERT INTO DocumentSpecialDataField VALUES (9, '001', 'CisloUverovehoUctu', 6, 'PaymentAccount', NULL, NULL, NULL, NULL)

INSERT INTO DocumentVariant VALUES (9, '001', 'CisloUverovehoUctu', 'A')
INSERT INTO DocumentVariant VALUES (9, '001', 'CisloUverovehoUctu', 'B')
INSERT INTO DocumentVariant VALUES (9, '001', 'CisloUverovehoUctu', 'C')
INSERT INTO DocumentVariant VALUES (9, '001', 'CisloUverovehoUctu', 'D')

DELETE FROM DocumentDynamicInputParameter WHERE DocumentId = 9 AND DynamicInputParameterId = 7