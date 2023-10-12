

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


# "Party must contain either LegalPerson or NaturalPerson."
json_bad_req_mail_mpss_empty_party_from = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {}
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
    "subject": "NS_test_MIKEL_bad_empty_party",
    "content": {
      "format": "application/mht",
      "language": "cs",
      "text": "json_bad_req_mail_mpss_empty_party_from"
    },
    "attachments": [],
    "identifier": {
      "identity": "992474q",
      "identityScheme": "1"
    }
  }


json_bad_req_mail_mpss_both_party_from = \
  {
    "from": {
      "value": "notification-service@mpss.cz",
      "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          },
          "naturalPerson": {
            "firstName": "Marek",
            "middleName": "Bozi",
            "surname": "Mikel"
          }}
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
    "subject": "NS_test_MIKEL_bad_both_party",
    "content": {
      "format": "application/mht",
      "language": "cs",
      "text": "json_bad_req_mail_mpss_both_party_from"
    },
    "attachments": [],
    "identifier": {
      "identity": "992474q",
      "identityScheme": "1"
    }
  }


#documentHash bez Hash
json_req_mail_mpss_documentHash_without_hash = \
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
      "text": "json_req_mail_mpss_documentHash_withnout_hash"
    },
    "attachments": [],
    "documentHash": {
    "hashAlgorithm": "SHA-3"
  }
  }


#documentHash bez hashAlgorithm
json_req_mail_mpss_documentHash_without_hashAlgorithm = \
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
      "text": "json_req_mail_mpss_documentHash_withonut_hashAlgorithm"
    },
    "attachments": [],
    "documentHash": {
    "hash": "a3f4b2e8967d3a2c1b5f4e6090d7c3b2a4f8e9d0c6b5a432e9d0b3a295f7c8e1"
  }
  }

#documentHash se špatným formátem hash
json_req_mail_mpss_documentHash_with_bad_hash = \
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
      "text": "json_req_mail_mpss_documentHash_bad_hash"
    },
    "attachments": [],
    "documentHash": {
    "hash": "z3f4b2e8967d3a2c1b5f4e6090d7c3b2a4f8e9d0c6b5a432e9d0b3a295f7c8e1",
    "hashAlgorithm": "SHA-3"
  }
  }


#documentHash bad hashAlgoritm
json_req_mail_mpss_documentHash_bad_hashAlgorithm = \
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
    "hashAlgorithm": "MMI-1989"
  }
  }

json_req_mail_mpss_bad_from_mpss = \
  {
    "from": {
      "value": "notification-service@relay.mpss.cz",
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


json_req_mail_mpss_bad_from = \
  {
    "from": {
      "value": "notification-service@gmail.com",
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
