
json_req_sms_full_template = \
{
  "phoneNumber": "00420607115686",
  "processingPriority": 1,
  "type": "TESTING_TEMPLATE",
  "placeholders": [
    {
      "key": "zadej",
      "value": "Mara testuje"
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
  "phoneNumber": "+420607115686",
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
  "phoneNumber": "+420607115686",
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
  "phoneNumber": "+420607115686",
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