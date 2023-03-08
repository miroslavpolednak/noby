
---------------------------
-- CLEAR DB
---------------------------
PRAGMA writable_schema = 1;
DELETE FROM sqlite_master;
PRAGMA writable_schema = 0;
VACUUM;
PRAGMA integrity_check;
---------------------------

-- SET foreign keys To OFF
PRAGMA foreign_keys = OFF;

CREATE TABLE TestData (TimeCreated TEXT, RecordOrder INTEGER, RecordSource TEXT, RecordData TEXT, PRIMARY KEY (TimeCreated, RecordOrder, RecordSource));

-- SET foreign keys To ON
PRAGMA foreign_keys = ON;
--PRAGMA foreign_keys;
