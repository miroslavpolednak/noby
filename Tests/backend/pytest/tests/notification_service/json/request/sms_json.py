# phone String E2E test
json_req_sms_basic_insg_e2e = \
    {
        "phoneNumber": "+420607115686",
        "processingPriority": 1,
        "type": "INSIGN_PROCESS",
        "text": "insg bez logování a odstraň háčky a čárky"
    }

# phone String
json_req_sms_basic_insg = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "INSIGN_PROCESS",
        "text": "e2e TEST insg bez logovani"
    }

json_req_sms_basic_kb_insg = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "KB_INSIGN_PROCESS",
        "text": "e2e TEST KB insg bez logovani"
    }

json_req_sms_basic_insg_uat = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "INSIGN_PROCESS",
        "text": "UAT insg bez logování a chci háčky a čárky"
    }

json_req_sms_basic_insg_fat = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "INSIGN_PROCESS",
        "text": "FAT insg bez logování a chci háčky a čárky"
    }

json_req_sms_basic_insg_sit = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "INSIGN_PROCESS",
        "text": "SIT insg bez logování a chci háčky a čárky"
    }

json_req_sms_sb = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "SB_NOTIFICATIONS_KB",
        "text": "SB test sms 2"
    }

# phone_object
json_req_sms_basic_phone_object = \
    {
        "phone": {
            "countryCode": "+420",
            "nationalNumber": "123456789"
        },
        "processingPriority": 1,
        "type": "INSIGN_PROCESS",
        "text": "insg s logovani"
    }

json_req_sms_logovani_kb_sb_E2E = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "SB_NOTIFICATIONS_AUDITED_KB",
        "text": "kb logovani SB",
        "customId": "log",
        "caseId": 303062934,
        "documentHash": {
            "hash": "a3f4b2e8967d3a2c1b5f4e6090d7c3b2a4f8e9d0c6b5a432e9d0b3a295f7c8e1",
            "hashAlgorithm": "SHA-3"
        }
    }

json_req_sms_logovani_mpss_sb = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "KB_INSIGN_PROCESS",
        "text": "kb logovani insg",
        "customId": "log"
    }

json_req_sms_logovani_kb_insg = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "SB_NOTIFICATIONS_AUDITED_MP",
        "text": "mpss logovani SB",
        "customId": "log"
    }

json_req_sms_bez_logovani_kb_sb = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "SB_NOTIFICATIONS_KB",
        "text": "kb bez logovani SB",
        "customId": "nelog"
    }

json_req_sms_bez_logovani_mpss_sb = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "SB_NOTIFICATIONS_MP",
        "text": "mpss bez logovani SB",
        "customId": "nelog"
    }

json_req_sms_basic_full = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "INSIGN_PROCESS",
        "text": "kratka sms bez cehokoliv vice",
        "identifier": {
            "identity": "992474q",
            "identityScheme": "2"
        },
        "customId": "CustomID_876",
        "documentId": "DocumentID_876"
    }

json_req_sms_mpss_archivator = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "ARCHIVATOR_RETENCE_OZNAMENI_MP",
        "text": "mpss archivator",
        "customId": "log"
    }

json_req_sms_kb_archivator = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "ARCHIVATOR_RETENCE_OZNAMENI_KB",
        "text": "kb archivator",
        "customId": "log"
    }

json_req_sms_bad_basic_without_identifier = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "INSIGN_PROCESS",
        "text": "kratka sms bez cehokoliv vice",
        "identifier": {},
        "customId": "CustomID_876",
        "documentId": "DocumentID_876"
    }

json_req_sms_bad_basic_without_identifier_scheme = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "INSIGN_PROCESS",
        "text": "kratka sms bez cehokoliv vice",
        "identifier": {
            "identity": "992474q"
        },
        "customId": "CustomID_876",
        "documentId": "DocumentID_876"
    }

json_req_sms_bad_basic_without_identifier_identity = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "INSIGN_PROCESS",
        "text": "kratka sms bez cehokoliv vice",
        "identifier": {
            "identityScheme": "2"
        },
        "customId": "CustomID_876",
        "documentId": "DocumentID_876"
    }

json_req_sms_basic_full_for_search = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "INSIGN_PROCESS",
        "text": "sms pro search",
        "identifier": {
            "identity": "992474q",
            "identityScheme": "2"
        },
        "customId": "CustomID_876",
        "documentId": "DocumentID_876",
        "caseId": 303062934
    }

json_req_sms_basic_epsy_kb = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "EPODPISY_SIGNED_DOCUMENT_KB",
        "text": "Testovací sms text epsy kb"
    }

json_req_sms_basic_epsy_mpss = \
    {
        "phoneNumber": "+420123456789",
        "processingPriority": 1,
        "type": "EPODPISY_SIGNED_DOCUMENT_MP",
        "text": "Testovací sms text epsy mpss"
    }

# phone case
json_req_sms_caseId = \
    {
        "phoneNumber": "+420607115686",
        "processingPriority": 1,
        "type": "INSIGN_PROCESS",
        "text": "caseId",
        "caseId": 303062934
    }

# phone case
json_req_sms_documentHash = \
    {
        "phoneNumber": "+420607115686",
        "processingPriority": 1,
        "type": "INSIGN_PROCESS",
        "text": "documentHash",
        "documentHash": {
            "hash": "a3f4b2e8967d3a2c1b5f4e6090d7c3b2a4f8e9d0c6b5a432e9d0b3a295f7c8e1",
            "hashAlgorithm": "SHA-256"
        }
    }
