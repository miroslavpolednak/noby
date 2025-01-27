

# from, to, legalPerson, bez priloh
json_req_mail_kb_basic = \
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
  "subject": "Předmět - test na KB s přílohou",
  "content": {
    "format": "UTF-8",
    "language": "cz",
    "text": "Testovací text pro testovací email s prilohou"
  },
  "attachments": []
}

# from, to, dcc, cc, replyTo, legalPerson, bez priloh
json_req_mail_kb_copies = \
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
  "bcc": [
    {
      "value": "jakub_vana@kb.cz",
      "party": {
        "legalPerson": {
          "name": "Kubik Vana"
        }
      }
    }
  ],
  "cc": [
    {
      "value": "marek.mikel@kb.cz",
      "party": {
        "legalPerson": {
          "name": "Mara Mikel"
        }
      }
    }
  ],
  "replyTo": {
     "value": "jakub.vana.reply@kb.cz",
     "party": {
       "legalPerson": {
         "name": "Notifikace"
       }
     }
   },
  "subject": "Předmět - test na KB bez přílohy",
  "content": {
    "format": "UTF-8",
    "language": "cz",
    "text": "Testovací text pro testovací email bez prilohy"
  },
  "attachments": []
}

# from, to, dcc, cc, replyTo, legalPerson, bez priloh
json_req_mail_kb_copies_revert = \
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
  "bcc": [
    {
      "value": "marek.mikel@kb.cz",
      "party": {
        "legalPerson": {
          "name": "Mara Mikel"
        }
      }
    }
  ],
  "cc": [

    {
      "value": "jakub_vana@kb.cz",
      "party": {
        "legalPerson": {
          "name": "Kubik Vana"
        }
      }
    }
  ],
  "replyTo": {
     "value": "jakub.vana.reply@kb.cz",
     "party": {
       "legalPerson": {
         "name": "Notifikace"
       }
     }
   },
  "subject": "Předmět - test na KB bez přílohy",
  "content": {
    "format": "UTF-8",
    "language": "cz",
    "text": "Testovací text pro testovací email bez prilohy"
  },
  "attachments": []
}

# from, to, bcc, cc, replyTo, naturalPerson, attachments
json_req_mail_kb_full_attachments = \
  {
    "from": {
      "value": "notification-service@kb.cz",
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
        "value": "jakub_vana@kb.cz",
        "party": {
        "naturalPerson": {
          "firstName": "Marek",
          "middleName": "Bozi",
          "surname": "Mikel"
        }
        }
      }
    ],
    "bcc": [
      {
        "value": "marek.mikel@kb.cz",
       "party": {
        "naturalPerson": {
          "firstName": "Marek",
          "middleName": "Bozi",
          "surname": "Mikel"
        }
        }
      }
    ],
    "cc": [
     {
        "value": "karel.nguyen-trong@kb.cz",
       "party": {
        "naturalPerson": {
          "firstName": "Marek",
          "middleName": "Bozi",
          "surname": "Mikel"
        }
        }
      }
    ],
    "replyTo": {
      "value": "marek.mikel_reply@kb.cz",
      "party": {
        "naturalPerson": {
          "firstName": "Marek",
          "middleName": "Bozi",
          "surname": "Mikel"
        }
      }
    },
    "subject": "Predmet_testy_pardon_LEGAL",
    "content": {
      "format": "UTF-8",
      "language": "cz",
      "text": "Sorry za spam. Overuji kopie a identifier. Marek"
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

# from, to, replyTo, legalPerson, attachments
json_req_mail_kb_basic_attachments = \
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
    "replyTo": {
      "value": "marek.mikel_reply@kb.cz",
      "party": {
        "legalPerson": {
          "name": "Notifikace"
        }
      }
    },
    "subject": "Predmet_testy_pardon_LEGAL KB",
    "content": {
      "format": "UTF-8",
      "language": "cz",
      "text": "Sorry za spam. Overuji kopie a identifier a KB. Marek"
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
      "identityScheme": "1"
    },
    "customId": "customId_123456",
    "documentId": "documentId_123456"
  }
