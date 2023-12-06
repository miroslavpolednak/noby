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
        "value": "jakub_vana@kb.cz",
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


# from, to, max priloh
json_req_mail_kb_max_attachments = \
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
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_attachment_kb",
    "content": {
      "format": "text/plain",
      "language": "cs",
      "text": "json_req_mail_kb_att"
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

#sender
json_req_mail_kb_sender_kb_attachment = \
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
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_sender_kb.cz",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "Marek Mikel"
    },
    "attachments": [{
        "filename": "attachment.txt",
        "binary": "77u/YXR0YWNobWVudCAx"
      }]
  }

json_req_mail_kb_sender_kb = \
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
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_sender_kb.cz",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "Marek Mikel"
    },
    "attachments": []
  }

json_req_mail_kb_sender_kb_sluzby = \
  {
    "from": {
      "value": "notification-service@kbsluzby.cz",
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
    "subject": "NS_test_MIKEL_sender_kbsluzby.cz",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "Marek Mikel"
    },
    "attachments": []
  }

json_req_mail_kb_sender_kbinfo = \
  {
    "from": {
      "value": "notification-service@kbinfo.cz",
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
    "subject": "NS_test_MIKEL_sender_kbinfo.cz",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "Marek Mikel"
    },
    "attachments": []
  }


#tests current content.format
json_req_mail_kb_basic_format_application_html = \
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
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_json_req_mail_kb_basic_format_application_html",
    "content": {
      "format": "application/html",
      "language": "cs",
      "text": "json_req_mail_kb_basic_format_application_html"
    },
    "attachments": []
  }

json_req_mail_kb_basic_format_text_html = \
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
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_json_req_mail_kb_basic_format_text_html",
    "content": {
      "format": "text/html",
      "language": "cs",
      "text": "<html><head></head><body>Testovaci text</body></html>"
    },
    "attachments": []
  }


json_req_mail_kb_basic_format_html = \
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
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_json_req_mail_kb_basic_format_html",
    "content": {
      "format": "html",
      "language": "cs",
      "text": "json_req_mail_kb_basic_format_html"
    },
    "attachments": []
  }

json_req_mail_kb_basic_format_text_plain = \
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
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_json_req_mail_kb_basic_format_text_plain",
    "content": {
      "format": "text/plain",
      "language": "cs",
      "text": "json_req_mail_kb_basic_format_text_plain"
    },
    "attachments": []
  }


json_req_mail_kb_basic_format_application_text = \
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
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_json_req_mail_kb_basic_format_application_text",
    "content": {
      "format": "application/text",
      "language": "cs",
      "text": "json_req_mail_kb_basic_format_application_text"
    },
    "attachments": []
  }


json_req_mail_kb_basic_content_format_application_mht = \
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
        "value": "marek.mikel@mpss.cz",
        "party": {
          "legalPerson": {
            "name": "Marek Mikel"
          }
        }
      }
    ],
    "subject": "NS_test_MIKEL_json_req_mail_kb_basic_format_application_mht",
    "content": {
      "format": "application/mht",
      "language": "cs",
       "text": "From: \"notification-service@kb.cz\"\\nSubject: Testovací MHT Soubor\\nDate: Wed, 1 Nov 2023 10:00:00 +0200\\nMIME-Version: 1.0\\nContent-Type: multipart/related; boundary=\"----=_NextPart_000_0000_01D3F6A2.BC237A30\"; type=\"text/html\"\\n\\n------=_NextPart_000_0000_01D3F6A2.BC237A30\\nContent-Type: text/html; charset=\"utf-8\"\\nContent-Transfer-Encoding: quoted-printable\\n\\n<!DOCTYPE html>\\n<html>\\n<head>\\n<title>Testovací Stránka</title>\\n</head>\\n<body>\\n<h1>Ahoj světe!</h1>\\n<p>Toto je testovací MHT obsah.</p>\\n</body>\\n</html>\\n\\n------=_NextPart_000_0000_01D3F6A2.BC237A30--"
    },
    "attachments": []
  }
