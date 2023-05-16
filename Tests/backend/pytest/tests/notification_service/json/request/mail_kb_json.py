# from, to, legalPerson, bez priloh
json_req_mail_kb_basic_legal = \
  {
    "from": {
      "value": "notification-service@kb.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "jakub_vana@ks.cz",
        "party": {
          "legalPerson": {
            "name": "Jakub Vana"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_basic_legal_kb",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_kb_basic_legal"
    },
    "attachments": []
  }




# negative - from, to, legalPerson, bez priloh
json_req_mail_kb_bad_11_attachments = \
  {
    "from": {
      "value": "notification-service@kb.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "jakub_vana@kb.cz",
        "party": {
          "legalPerson": {
            "name": "Kubik Vana"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_bad_11_attachments_kb",
    "content": {
      "format": "application/html",
      "language": "cz",
      "text": "json_req_mail_mpss_bad_11_attachments_kb"
    },
    "attachments": [
      {
        "filename": "1.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      },
      {
        "filename": "2.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      },
{
        "filename": "3.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      },
      {
        "filename": "4.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      },
{
        "filename": "5.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      },
{
        "filename": "6.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      },
{
        "filename": "7.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      },
{
        "filename": "8.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      },{
        "filename": "9.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      },
{
        "filename": "10.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      },
{
        "filename": "11.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      }
    ]
  }