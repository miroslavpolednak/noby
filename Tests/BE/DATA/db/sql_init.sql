
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

CREATE TABLE TestData (RecordSource INTEGER, RecordOrder INTEGER, RecordEnvironments INTEGER, TimeCreated TEXT, RecordData TEXT, PRIMARY KEY (RecordSource, RecordOrder));

-- SET foreign keys To ON
PRAGMA foreign_keys = ON;
--PRAGMA foreign_keys;
