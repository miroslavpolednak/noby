
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
        "value": "martin.heimlich@mpss.cz",
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
        "value": "jakub.vana@mpss.cz",
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
    "subject": "NS_test_MIKEL_atch_mpss",
    "content": {
      "format": "application/mht",
      "language": "cs",
      "text": "json_req_mail_mpss_atch"
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
      "text": "<html><head></head><body>Testovaci text</body></html>"
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

json_req_mail_mpss_basic_format_text_plain = \
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


json_req_mail_mpss_basic_format_application_text = \
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
    "subject": "NS_test_MIKEL_json_req_mail_mpss_basic_format_application_text",
    "content": {
      "format": "application/text",
      "language": "cs",
      "text": "json_req_mail_mpss_basic_format_application_text"
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
    "subject": "NS_test_MIKEL_json_req_mail_mpss_basic_format_application_mht",
    "content": {
      "format": "application/mht",
      "language": "cs",
      "text": "From: \"notification-service@mpss.cz\"\\nSubject: Testovací MHT Soubor\\nDate: Wed, 1 Nov 2023 10:00:00 +0200\\nMIME-Version: 1.0\\nContent-Type: multipart/related; boundary=\"----=_NextPart_000_0000_01D3F6A2.BC237A30\"; type=\"text/html\"\\n\\n------=_NextPart_000_0000_01D3F6A2.BC237A30\\nContent-Type: text/html; charset=\"utf-8\"\\nContent-Transfer-Encoding: quoted-printable\\n\\n<!DOCTYPE html>\\n<html>\\n<head>\\n<title>Testovací Stránka</title>\\n</head>\\n<body>\\n<h1>Ahoj světe!</h1>\\n<p>Toto je testovací MHT obsah.</p>\\n</body>\\n</html>\\n\\n------=_NextPart_000_0000_01D3F6A2.BC237A30--"
    },
    "attachments": []
  }


json_req_mail_mpss_null_party_from = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": None
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
    "subject": "NS_test_MIKEL_null_party",
    "content": {
      "format": "application/mht",
      "language": "cs",
      "text": "json_req_mail_mpss_null_party_from"
    },
    "attachments": [],
    "identifier": {
      "identity": "992474q",
      "identityScheme": "1"
    }
  }


json_req_mail_mpss_without_party_from = \
  {
    "from": {
      "value": "notification-service@mpss.cz"
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
    "subject": "NS_test_MIKEL_without_party",
    "content": {
      "format": "application/mht",
      "language": "cs",
      "text": "json_req_mail_mpss_without_party_from"
    },
    "attachments": [],
    "identifier": {
      "identity": "992474q",
      "identityScheme": "1"
    }
  }

#Case Id real
json_req_mail_mpss_case = \
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
    "subject": "NS_test_MIKEL",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_mpss_case"
    },
    "attachments": [],
    "caseId": 303062934
  }

#documentHash SHA-256
json_req_mail_mpss_documentHash_SHA_256 = \
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
    "subject": "NS_test_MIKEL",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_mpss_documentHash_SHA_256"
    },
    "attachments": [],
    "documentHash": {
    "hash": "a3f4b2e8967d3a2c1b5f4e6090d7c3b2a4f8e9d0c6b5a432e9d0b3a295f7c8e1",
    "hashAlgorithm": "SHA-256"
  }
  }

#documentHash SHA-384
json_req_mail_mpss_documentHash_SHA_384 = \
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
    "subject": "NS_test_MIKEL",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_mpss_documentHash_SHA_384"
    },
    "attachments": [],
    "documentHash": {
    "hash": "a3f4b2e8967d3a2c1b5f4e6090d7c3b2a4f8e9d0c6b5a432e9d0b3a295f7c8e1",
    "hashAlgorithm": "SHA-384"
  }
  }

#documentHash SHA-512
json_req_mail_mpss_documentHash_SHA_512 = \
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
    "subject": "NS_test_MIKEL",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_mpss_documentHash_SHA_512"
    },
    "attachments": [],
    "documentHash": {
    "hash": "a3f4b2e8967d3a2c1b5f4e6090d7c3b2a4f8e9d0c6b5a432e9d0b3a295f7c8e1",
    "hashAlgorithm": "SHA-512"
  }
  }

#documentHash SHA-3
json_req_mail_mpss_documentHash_SHA_3 = \
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
    "subject": "NS_test_MIKEL",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_mpss_documentHash_SHA_3"
    },
    "attachments": [],
    "documentHash": {
    "hash": "a3f4b2e8967d3a2c1b5f4e6090d7c3b2a4f8e9d0c6b5a432e9d0b3a295f7c8e1",
    "hashAlgorithm": "SHA-3"
  }
  }

#sender
json_req_mail_mpss_sender_mpss = \
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
    "subject": "NS_test_MIKEL_sender_mpss.cz",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "Marek Mikel"
    },
    "attachments": []
  }

json_req_mail_mpss_sender_mpss_info = \
  {
    "from": {
      "value": "notification-service@mpss-info.cz",
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
    "subject": "NS_test_MIKEL_sender_mpss-info.cz",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "Marek Mikel"
    },
    "attachments": []
  }


json_req_mail_mpss_sender_vsskb = \
  {
    "from": {
      "value": "notification-service@vsskb.cz",
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
    "subject": "NS_test_MIKEL_sender_vsskb.cz",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "Marek Mikel"
    },
    "attachments": []
  }