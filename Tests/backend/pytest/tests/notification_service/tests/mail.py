import itertools

import pytest
import requests

from ..conftest import URLS
from ..json.request.mail_kb_json import json_req_mail_kb_bad_11_attachments
from ..json.request.mail_mpss_json import json_req_mail_mpss_basic_legal, json_req_mail_mpss_basic_natural, \
    json_req_mail_mpss_full_attachments, json_req_mail_mpss_full_natural, json_req_mail_mpss_bad_natural_legal, \
    json_req_mail_mpss_max_attachments, json_req_mail_mpss_bad_11_attachments, \
    json_req_mail_bad_identifier_identity_mpss_basic, \
    json_req_mail_bad_identifier_mpss_basic, json_req_mail_bad_identifier_identity_mpss_basic, \
    json_req_mail_bad_identifier_scheme_mpss_basic, json_req_mail_mpss_bad_format_language, \
    json_req_mail_mpss_bad_content_format_text, json_req_mail_mpss_basic_format_html, \
    json_req_mail_mpss_negative_basic_format_text_plain, json_req_mail_mpss_basic_format_text_html, \
    json_req_mail_mpss_basic_format_application_html, json_req_mail_mpss_basic_content_format_application_mht


#zákkladní test
@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST", "XX_SB_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_basic_legal, json_req_mail_mpss_basic_natural, json_req_mail_mpss_full_attachments,
                                       json_req_mail_mpss_full_natural, ])
def test_mail(url_name,  auth_params, auth, json_data):
    """kladny test"""

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

#TODO:content.format test - az po Drobne zmeny 3
@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_basic_format_application_html,
                                       json_req_mail_mpss_basic_format_text_html,
                                       json_req_mail_mpss_basic_format_html,
                                       json_req_mail_mpss_basic_content_format_application_mht])
def test_mail_content_format(url_name,  auth_params, auth, json_data):
    """kladny test"""

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

#test variant
@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_max_attachments])
def test_mail_max_attachments(url_name,  auth_params, auth, json_data):
    """max priloh klady test"""

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


#negativní testy
@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_bad_format_language])
def test_mail_negative_format_language(url_name,  auth_params, auth, json_data):
    """negativní test pro test jazyka a formatu"""

    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    assert resp.status_code == 400
    error_message = resp.json()['errors']['319'][0]
    assert 'Allowed values for Language: cs,en.' in error_message


@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_bad_11_attachments, json_req_mail_kb_bad_11_attachments])
def test_mail_negative_11_attachments(url_name,  auth_params, auth, json_data):
    """max priloh klady test, ale MCS zařízne"""

    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    assert resp.status_code == 400
    error_message = resp.json()['errors']['351'][0]
    assert 'Maximum count of Attachments is 10' in error_message


#TODO: dodelat assrty pro errors == zatím není vyvinuta hláška  bude vyvinuto v drobné změny 3
@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_bad_identifier_mpss_basic,
                                     json_req_mail_bad_identifier_scheme_mpss_basic,
                                     json_req_mail_bad_identifier_identity_mpss_basic])
def test_mail_negative_identifier_request(auth_params, auth, json_data, url_name):
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
    assert resp['status'] == 400
    print(resp)
    error_message = resp.json()['errors']['319'][0]


#TODO: dodelat assrty pro errors hlášku, bude vyvinuto v drobné změny 3
@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_bad_content_format_text,
                                       json_req_mail_mpss_negative_basic_format_text_plain])
def test_mail_negative_content_format(url_name,  auth_params, auth, json_data):
    """negativní test pro test jazyka a formatu"""

    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    assert resp.status_code == 400
    error_message = resp.json()['errors']['317'][0]
    assert 'Allowed values for Format: application/html,application/mht,html,text/html.' in error_message

#pro testy zabezpeceni, jake sms jsou mozne odespilat pres urcite uzivatele - pouzita vnorena parametrizace
@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST", "XX_NOBY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_basic_legal])
def test_mail_bad_auth(auth_params, auth, json_data, url_name):
    username = auth[0]
    password = auth[1]
    session = requests.session()
    resp = session.post(
        URLS[url_name] + "/v1/notification/email",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    assert resp.status_code == 403


@pytest.mark.parametrize("url_name", ["dev_url"])
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_bad_natural_legal])
def test_mail_bad_name(auth_params, auth, json_data, url_name):
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
    assert resp['status'] == 400
    print(resp)
    errors = resp.get('errors', {})
    assert any('FirstName required' in error for error_list in errors.values() for error in error_list)
    assert any('Surname required' in error for error_list in errors.values() for error in error_list)
    assert any('Name required' in error for error_list in errors.values() for error in error_list)
