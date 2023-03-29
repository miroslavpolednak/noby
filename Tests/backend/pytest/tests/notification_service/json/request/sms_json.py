#phone String
json_req_sms_basic= \
{
  "phoneNumber": "+420607115686",
  "processingPriority": 1,
  "type": "RETENTION",
  "text": "insg bez logování a chci háčky a čárky"
}

json_req_sms_sb= \
{
  "phoneNumber": "+420607115686",
  "processingPriority": 1,
  "type": "TEST_SB",
  "text": "SB test sms 2"
}

#phone_object
json_req_sms_basic_phone_object = \
{
  "phone": {
    "countryCode": "+420",
    "nationalNumber": "607115686"
  },
  "processingPriority": 1,
  "type": "RETENTION",
  "text": "insg bez logovani"
}

json_req_sms_logovani= \
{
  "phoneNumber": "+420607115686",
  "processingPriority": 1,
  "type": "TEST_INSG",
  "text": "insg s logovani"
}

json_req_sms_bez_logovani= \
{
  "phoneNumber": "+420607115686",
  "processingPriority": 1,
  "type": "TEST_INSG_NELOG",
  "text": "insg s logovani"
}

json_req_sms_basic_full= \
{
  "phoneNumber": "+420607115686",
  "processingPriority": 1,
  "type": "RETENTION",
  "text": "kratka sms bez cehokoliv vice",
  "identifier": {
    "identity": "992474q",
    "identityScheme": "2"
  },
  "customId": "CustomID_876",
  "documentId": "DocumentID_876"
}

json_req_sms_basic_full_for_search= \
{
  "phoneNumber": "+420607115686",
  "processingPriority": 1,
  "type": "RETENTION",
  "text": "sms pro search",
  "identifier": {
    "identity": "992474q",
    "identityScheme": "2"
  },
  "customId": "CustomID_876",
  "documentId": "DocumentID_876"
}

json_req_sms_basic_epsy = \
{
  "phoneNumber": "+420607115686",
  "processingPriority": 1,
  "type": "TEST_EPSY",
  "text": "Testovací sms text epsy s logovanim"
}

json_req_sms_basic_insg = \
{
  "phoneNumber": "+420607115686",
  "processingPriority": 1,
  "type": "TEST_INSG",
  "text": "Testovací sms insg s logovanim"
}