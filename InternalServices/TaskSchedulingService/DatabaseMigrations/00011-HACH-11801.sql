delete from ScheduleJob where ScheduleJobId in ('A61AA97D-05C4-4D8F-B488-2EE35B5D2A9C','6290CA85-DEAE-4299-AB63-BF3973D3859B');
delete from ScheduleTrigger where ScheduleJobId in ('A61AA97D-05C4-4D8F-B488-2EE35B5D2A9C','6290CA85-DEAE-4299-AB63-BF3973D3859B');
update ScheduleTrigger set JobData='["CB_CmTrTinFormat", "CB_CmTrTinCountry", "CB_SourceOfEarningsVsProfessionCategory", "CB_CmEpProfessionCategory", "CB_StandardMethodOfArrAcceptanceByNPType", "MAP_CB_CmEpProfessionCategory_CB_CmEpProfession", "CB_CmCoPhoneIdc", "CB_IdentificationMethodType", "CB_HyporetenceResponse"]' where ScheduleTriggerId='2FE9145E-05CD-4E04-98CD-A9DD0E995BE6';
delete from SqlQuery where SqlQueryId in ('CountryCodePhoneIdc', 'IdentificationSubjectMethods', 'TinFormatsByCountry', 'TinNoFillReasonsByCountry');
