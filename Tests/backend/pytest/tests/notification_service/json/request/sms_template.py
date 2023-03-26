
json_req_sms_full_template = \
{
  "phoneNumber": "00420607115686",
  "processingPriority": 1,
  "type": "TESTING_TEMPLATE",
  "placeholders": [
    {
      "key": "date_from",
      "value": "dneska"
    },
    {
      "key": "date_to",
      "value": "nekonecna a jeste dal"
    },
{
      "key": "count",
      "value": "1000000"
    }
  ],
  "identifier": {
    "identity": "992474q",
    "identityScheme": "2"
  },
  "customId": "123546",
  "documentId": "789456"
}


json_req_sms_basic_template = \
{
  "phoneNumber": "+420607115686",
  "processingPriority": 1,
  "type": "RETENTION_TEMPLATE",
  "placeholders": [
    {
      "key": "environment",
      "value": "test"
    }
  ]
}


json_req_sms_full_template_uat = \
{
  "phoneNumber": "00420607115686",
  "processingPriority": 1,
  "type": "TESTING_TEMPLATE",
  "placeholders": [
    {
      "key": "date_from",
      "value": "uat_dneska"
    },
    {
      "key": "date_to",
      "value": "nekonecna a jeste dal"
    },
{
      "key": "count",
      "value": "1000000"
    }
  ],
  "identifier": {
    "identity": "992474q",
    "identityScheme": "2"
  },
  "customId": "123546",
  "documentId": "789456"
}


json_req_sms_basic_template_uat = \
{
  "phoneNumber": "+420607115686",
  "processingPriority": 1,
  "type": "RETENTION_TEMPLATE",
  "placeholders": [
    {
      "key": "environment",
      "value": "test_uat"
    }
  ]
}