INSERT INTO DocumentDynamicInputParameter VALUES (9, '001', 2)

INSERT INTO DocumentSpecialDataField VALUES (9, '001', 'CisloUverUctu', 6, 'PaymentAccount', NULL, NULL, NULL, NULL)

INSERT INTO DocumentVariant VALUES (9, '001', 'CisloUverUctu', 'A')
INSERT INTO DocumentVariant VALUES (9, '001', 'CisloUverUctu', 'B')
INSERT INTO DocumentVariant VALUES (9, '001', 'CisloUverUctu', 'C')
INSERT INTO DocumentVariant VALUES (9, '001', 'CisloUverUctu', 'D')

DELETE FROM DocumentDynamicInputParameter WHERE DocumentId = 9 AND DynamicInputParameterId = 7