
# from, to, legalPerson, bez priloh
json_req_mail_mpss_basic_legal = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_basic_legal",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_mpss_basic_legal"
    },
    "attachments": []
  }


# from, to, naturalPerson, bez priloh
json_req_mail_mpss_basic_natural = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "naturalPerson": {
        "firstName": "Marek",
        "middleName": "Bozi",
        "surname": "Mikel"
      }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
        "naturalPerson": {
        "firstName": "Marek",
        "middleName": "Bozi",
        "surname": "Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_basic_natural",
    "content": {
      "format": "application/mht",
      "language": "cs",
      "text": "json_req_mail_mpss_basic_natural"
    },
    "attachments": []
  }

# from, to, bcc, cc, replyTo, naturalPerson
json_req_mail_mpss_full_natural = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "naturalPerson": {
          "firstName": "Marek",
          "middleName": "Bozi",
          "surname": "Mikel"
        }
      }
    },
    "to": [
      {
        "value": "jakub.vana@mpss.cz",
        "party": {
          "naturalPerson": {
            "firstName": "Baru",
            "middleName": "Mikel",
            "surname": "Krsmaru"
          }
        }
      }
    ],
    "bcc": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "naturalPerson": {
            "firstName": "BaruBCC",
            "middleName": "MikelBCC",
            "surname": "KrsmaruBCC"
          }
        }
      }
    ],
    "cc": [
      {
        "value": "karel.nguyen-trong@mpss.cz",
        "party": {
          "naturalPerson": {
            "firstName": "BaruCC",
            "middleName": "MikelCC",
            "surname": "KrsmaruCC"
          }
        }
      }
    ],
    "replyTo": {
      "value": "marek.mikel_reply@mpss.cz",
      "party": {
        "naturalPerson": {
          "firstName": "MarekReply",
          "middleName": "MikelReply",
          "surname": "NovakReply"
        }
      }
    },
    "subject": "NS_test_MIKEL_full_natural",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_mpss_full_natural"
    },
    "attachments": [],
    "identifier": {
      "identity": "992474q",
      "identityScheme": "1"
    },
    "customId": "customId_123456",
    "documentId": "documentId_123456"
  }

# from, to, bcc, cc, replyTo, legalPerson, attachments
json_req_mail_mpss_full_attachments = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
     "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
        }
      }
    ],
    "bcc": [
      {
        "value": "marek.mikel@mpss.cz",
       "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
        }
      }
    ],
    "cc": [
     {
        "value": "karel.nguyen-trong@mpss.cz",
       "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
        }
      }
    ],
    "replyTo": {
      "value": "marek.mikel_reply@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "subject": "NS_test_MIKEL_full_attachments",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_mpss_full_attachments"
    },
    "attachments": [
      {
        "filename": "attachment.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      },
      {
        "filename": "image.png",
        "binary": "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"
      }
    ],
    "identifier": {
      "identity": "992472q",
      "identityScheme": "2"
    },
    "customId": "customId_123456",
    "documentId": "documentId_123456"
  }


# from, to, max priloh
json_req_mail_mpss_max_attachments = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_basic_legal",
    "content": {
      "format": "application/mht",
      "language": "cs",
      "text": "json_req_mail_mpss_basic_legal"
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
      },
      {
        "filename": "9.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      },
{
        "filename": "10.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      }
    ]
  }

#tests current content.format
json_req_mail_mpss_basic_format_application_html = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_json_req_mail_mpss_basic_format_application_html",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_mpss_basic_format_application_html"
    },
    "attachments": []
  }

json_req_mail_mpss_basic_format_text_html = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_json_req_mail_mpss_basic_format_text_html",
    "content": {
      "format": "text/html",
      "language": "cs",
      "text": "json_req_mail_mpss_basic_format_text_html"
    },
    "attachments": []
  }

json_req_mail_mpss_negative_basic_format_text_plain = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_json_req_mail_mpss_basic_format_text_plain",
    "content": {
      "format": "text/plain",
      "language": "cs",
      "text": "json_req_mail_mpss_basic_format_text_plain"
    },
    "attachments": []
  }

json_req_mail_mpss_basic_format_html = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_json_req_mail_mpss_basic_format_html",
    "content": {
      "format": "html",
      "language": "cs",
      "text": "json_req_mail_mpss_basic_format_html"
    },
    "attachments": []
  }

# ----------------------------------BAD REQUESTS
# obsahuje špatně format a jazyk
json_req_mail_mpss_bad_format_language = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_basic_legal",
    "content": {
      "format": "text/html",
      "language": "cz",
      "text": "json_req_mail_mpss_bad_format_language"
    },
    "attachments": []
  }

# obsahuje špatně content format
json_req_mail_mpss_bad_content_format_text = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_basic_legal",
    "content": {
      "format": "text",
      "language": "cs",
      "text": "json_req_mail_mpss_bad_content_format"
    },
    "attachments": []
  }

json_req_mail_mpss_basic_content_format_application_mht = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_basic_legal",
    "content": {
      "format": "text/html",
      "language": "cs",
      "text": "json_req_mail_mpss_content_format_application_mht"
    },
    "attachments": []
  }

# bez parameters identifiers
json_req_mail_bad_identifier_mpss_basic = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_basic_legal",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_bad_identifier_mpss_basic"
    },
  "identifier": {},
    "attachments": []
  }

# withnout parameters identifier.identityScheme
json_req_mail_bad_identifier_scheme_mpss_basic = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_basic_legal",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_bad_identifier_scheme_mpss_basic"
    },
  "identifier": {
    "identity": "992472q"
  },
    "attachments": []
  }

# withnout parameters identifier.identity
json_req_mail_bad_identifier_identity_mpss_basic = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_basic_legal",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_bad_identifier_identity_mpss_basi"
    },
  "identifier": {
    "identityScheme": "2"
  },
    "attachments": []
  }

# obsahuje špatně format a jazyk
json_req_mail_mpss_bad_format_content = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_basic_legal",
    "content": {
      "format": "text",
      "language": "cs",
      "text": "spatny content.format"
    },
    "attachments": []
  }

# more than ok attachments
json_req_mail_mpss_bad_11_attachments = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_bad_11_attachments",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_mpss_bad_11_attachments"
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
      },
      {
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


# fucker naturalPerson
json_req_mail_mpss_bad_natural_legal = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
        "naturalPerson": {}
      }
    },
    "to": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {}
        }
      }
    ],
    "bcc": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "naturalPerson": {}
        }
      }
    ],
    "cc": [
      {
        "value": "marek.mikel@mpss.cz",
        "party": {
          "naturalPerson": {}
        }
      }
    ],
    "replyTo": {
      "value": "marek.mikel_reply@mpss.cz",
      "party": {
        "naturalPerson": {}
      }
    },
    "subject": "NS_test_MIKEL_bad_natural_legal",
    "content": {
      "format": "application/mht",
      "language": "cs",
      "text": "json_req_mail_mpss_bad_natural_legal"
    },
    "attachments": [],
    "identifier": {
      "identity": "992474q",
      "identityScheme": "1"
    },
    "customId": "customId_123456",
    "documentId": "documentId_123456"
  }


