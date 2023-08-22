
json_req_sms_full_template_e2e = \
{
  "phoneNumber": "00420607115686",
  "processingPriority": 1,
  "type": "TESTING_TEMPLATE",
  "placeholders": [
    {
      "key": "zadej",
      "value": "kamarade E2E"
    }
  ],
  "identifier": {
    "identity": "992474q",
    "identityScheme": "2"
  },
  "customId": "CustomID_123546",
  "documentId": "DocumentID_789456"
}

json_req_sms_full_template = \
{
  "phoneNumber": "00420123456789",
  "processingPriority": 1,
  "type": "TESTING_TEMPLATE",
  "placeholders": [
    {
      "key": "zadej",
      "value": "kamarade"
    }
  ],
  "identifier": {
    "identity": "992474q",
    "identityScheme": "2"
  },
  "customId": "CustomID_123546",
  "documentId": "DocumentID_789456"
}


json_req_real_sms_full_template = \
{
  "phoneNumber": "00420607115686",
  "processingPriority": 1,
  "type": "TESTING_TEMPLATE",
  "placeholders": [
    {
      "key": "zadej",
      "value": "jsem automat."
    }
  ],
  "identifier": {
    "identity": "992474q",
    "identityScheme": "2"
  },
  "customId": "CustomID_123546",
  "documentId": "DocumentID_789456"
}


json_req_sms_template_bad_basic_without_identifier= \
{
  "phoneNumber": "+420123456789",
  "processingPriority": 1,
  "type": "TESTING_TEMPLATE",
  "placeholders": [
    {
      "key": "zadej",
      "value": "Mara testuje"
    }
  ],
  "text": "kratka sms bez cehokoliv vice",
  "identifier": {},
  "customId": "CustomID_876",
  "documentId": "DocumentID_876"
}

json_req_sms_template_bad_basic_without_identifier_scheme= \
{
  "phoneNumber": "+420123456789",
  "processingPriority": 1,
  "type": "TESTING_TEMPLATE",
  "placeholders": [
    {
      "key": "zadej",
      "value": "Mara testuje"
    }
  ],
  "text": "kratka sms bez cehokoliv vice",
    "identifier": {
    "identity": "992474q"
  },
  "customId": "CustomID_876",
  "documentId": "DocumentID_876"
}

json_req_sms_template_bad_basic_without_identifier_identity= \
{
  "phoneNumber": "+420123456789",
  "processingPriority": 1,
  "type": "TESTING_TEMPLATE",
  "placeholders": [
    {
      "key": "zadej",
      "value": "Mara testuje"
    }
  ],
  "text": "kratka sms bez cehokoliv vice",
    "identifier": {
    "identityScheme": "2"
  },
  "customId": "CustomID_876",
  "documentId": "DocumentID_876"
}