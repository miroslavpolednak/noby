USE [OfferService]
CREATE USER [kb_dwh] FOR LOGIN [kb_dwh]
GRANT ALTER ON SCHEMA::[bdp] TO [kb_dwh]
GRANT CONTROL ON SCHEMA::[bdp] TO [kb_dwh]
GRANT DELETE ON SCHEMA::[bdp] TO [kb_dwh]
GRANT INSERT ON SCHEMA::[bdp] TO [kb_dwh]
GRANT SELECT ON SCHEMA::[bdp] TO [kb_dwh]
GRANT UPDATE ON SCHEMA::[bdp] TO [kb_dwh]
GO