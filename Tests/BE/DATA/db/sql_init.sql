
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

CREATE TABLE TestData ([Source] INTEGER NOT NULL, [Order] INTEGER NOT NULL, [DataJson] TEXT NOT NULL, [TestEnvironments] INTEGER NOT NULL, [TestLayers] INTEGER NOT NULL, [TestTypes] INTEGER NOT NULL, [TimeCreated] TEXT NOT NULL, [SourceName] TEXT NULL, PRIMARY KEY ([Source], [Order]));

-- SET foreign keys To ON
PRAGMA foreign_keys = ON;
--PRAGMA foreign_keys;
