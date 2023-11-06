import time

import pytest
import requests
import urllib3

from Tests.backend.pytest.tests.notification_service.conftest import URLS

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

from ..json.request.mail_kb_json import json_req_mail_kb_max_attachments, json_req_mail_kb_basic_legal, \
    json_req_mail_kb_sender_kb, json_req_mail_kb_sender_kb_attachment, json_req_mail_kb_basic_format_application_html, \
    json_req_mail_kb_basic_content_format_application_mht, json_req_mail_kb_basic_format_application_text, \
    json_req_mail_kb_basic_format_html, json_req_mail_kb_basic_format_text_html, \
    json_req_mail_kb_basic_format_text_plain
from ..json.request.mail_mpss_json import json_req_mail_mpss_basic_legal, json_req_mail_mpss_basic_natural, \
    json_req_mail_mpss_full_attachments, json_req_mail_mpss_full_natural, \
    json_req_mail_mpss_basic_format_html, \
    json_req_mail_mpss_basic_format_text_html, json_req_mail_mpss_basic_format_application_html, \
    json_req_mail_mpss_basic_content_format_application_mht, json_req_mail_mpss_null_party_from, \
    json_req_mail_mpss_without_party_from, json_req_mail_mpss_case, \
    json_req_mail_mpss_documentHash_SHA_256, json_req_mail_mpss_documentHash_SHA_3, \
    json_req_mail_mpss_documentHash_SHA_512, json_req_mail_mpss_documentHash_SHA_384, \
    json_req_mail_mpss_basic_format_text_plain, json_req_mail_mpss_basic_format_application_text, \
    json_req_mail_mpss_max_attachments, \
    json_req_mail_mpss_sender_mpss, json_req_mail_mpss_sender_vsskb \


@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_basic_legal,
                                       json_req_mail_mpss_basic_natural,
                                       json_req_mail_kb_basic_legal])
def test_mail(ns_url, auth_params, auth, json_data):
    """kladny test"""
    url_name = ns_url["url_name"]
    url = ns_url["url"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    notification = resp.json()
    print(notification)
    assert "notificationId" in notification
    notification_id = notification["notificationId"]
    assert notification_id != ""

    assert 'strict-transport-security' in resp.headers, \
        'Expected "strict-transport-security" to be in headers'


# základní test
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_full_attachments, json_req_mail_mpss_full_natural])
def test_mail_full(ns_url, auth_params, auth, json_data):
    """kladny test"""
    url_name = ns_url["url_name"]
    url = ns_url["url"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    notification = resp.json()
    print(notification)
    assert "notificationId" in notification
    notification_id = notification["notificationId"]
    assert notification_id != ""

    assert 'strict-transport-security' in resp.headers, \
        'Expected "strict-transport-security" to be in headers'


@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [
                                       json_req_mail_mpss_basic_content_format_application_mht,
                                                                              ])
def test_mail_content_format(ns_url, auth_params, auth, json_data):
    """kladny test"""
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    notification_id = resp["notificationId"]
    assert notification_id != ""

    # Pro druhý GET request potřebujete přihlašovací údaje NOBY uživatele
    noby_username = "XX_NOBY_RMT_USR_TEST"
    noby_password = auth_params[noby_username]

    session = requests.session()
    resp = session.get(
        URLS[url_name] + f"/v1/notification/result/{notification_id}",
        json=json_data,
        auth=(noby_username, noby_password),
        verify=False
    )
    resp = resp.json()
    assert resp['notificationId'] == notification_id
    valid_states = ['Sent', 'InProgress']
    assert resp['state'] in valid_states, f"Invalid state: {resp['state']}, expected one of {valid_states}"
    assert resp['channel'] == 'Email'
    assert len(resp['errors']) == 0
    assert 'createdBy' in resp

    time.sleep(15)

    # vola GET opet, abz si overil doruceni
    session = requests.session()
    resp = session.get(
        URLS[url_name] + f"/v1/notification/result/{notification_id}",
        json=json_data,
        auth=(noby_username, noby_password),
        verify=False
    )
    resp = resp.json()
    assert resp['notificationId'] == notification_id
    assert resp['state'] == 'Sent'


# test variant
@pytest.mark.parametrize("auth", ["XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_max_attachments,
                                       json_req_mail_kb_max_attachments])
def test_mail_max_attachments(ns_url, auth_params, auth, json_data):
    """max priloh klady test"""
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    notification_id = resp["notificationId"]
    assert notification_id != ""


@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_null_party_from,
                                       json_req_mail_mpss_without_party_from]
                         )
def test_mail_party(auth_params, auth, json_data, ns_url):
    url_name = ns_url["url_name"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    resp = resp.json()
    print(resp)
    assert "notificationId" in resp
    notification_id = resp["notificationId"]
    assert notification_id != ""


# základní test case id
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_case])
def test_mail_caseId(ns_url, auth_params, auth, json_data):
    """kladny test"""
    url_name = ns_url["url_name"]
    url = ns_url["url"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    notification = resp.json()
    print(notification)
    assert "notificationId" in notification
    notification_id = notification["notificationId"]
    assert notification_id != ""

    assert 'strict-transport-security' in resp.headers, \
        'Expected "strict-transport-security" to be in headers'


# základní test documentHash
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_documentHash_SHA_256,
                                       json_req_mail_mpss_documentHash_SHA_3,
                                       json_req_mail_mpss_documentHash_SHA_512,
                                       json_req_mail_mpss_documentHash_SHA_384],
                         ids=[
                             "json_req_mail_mpss_documentHash_SHA_256",
                             "json_req_mail_mpss_documentHash_SHA_3",
                             "json_req_mail_mpss_documentHash_SHA_512",
                             "json_req_mail_mpss_documentHash_SHA_384"
                         ]
                         )
def test_mail_documentHash(ns_url, auth_params, auth, json_data):
    """kladny test"""
    url_name = ns_url["url_name"]
    url = ns_url["url"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    notification = resp.json()
    print(notification)
    assert "notificationId" in notification
    notification_id = notification["notificationId"]
    assert notification_id != ""

    assert 'strict-transport-security' in resp.headers, \
        'Expected "strict-transport-security" to be in headers'


#zatím bez json_req_mail_mpss_sender_vsskb a json_req_mail_kb_sender_kb_sluzby
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_sender_mpss,
                                       json_req_mail_kb_sender_kb,
                                        json_req_mail_kb_sender_kb_attachment
                                       ],
                         ids=[
                             "json_req_mail_mpss_sender_mpss",
                             "json_req_mail_kb_sender_kb",
                            "json_req_mail_kb_sender_kb_attachment"
                         ]
                         )
def test_mail_sender(ns_url, auth_params, auth, json_data):
    """kladny test"""
    url_name = ns_url["url_name"]
    url = ns_url["url"]
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    notification = resp.json()
    print(notification)
    assert "notificationId" in notification
    notification_id = notification["notificationId"]
    assert notification_id != ""

    assert 'strict-transport-security' in resp.headers, \
        'Expected "strict-transport-security" to be in headers'
