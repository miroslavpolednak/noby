import itertools

import pytest
import requests
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
from ..conftest import URLS
from ..json.request.mail_kb_json import json_req_mail_kb_bad_11_attachments, json_req_mail_kb_basic_legal
from ..json.request.mail_mpss_json import json_req_mail_mpss_basic_legal
from ..json.request.mail_mpss_neg_json import json_bad_req_mail_mpss_empty_party_from, \
    json_bad_req_mail_mpss_both_party_from, \
    json_req_mail_mpss_bad_11_attachments, \
    json_req_mail_bad_identifier_mpss_basic, json_req_mail_bad_identifier_identity_mpss_basic, \
    json_req_mail_bad_identifier_scheme_mpss_basic, json_req_mail_mpss_bad_format_language, \
    json_req_mail_mpss_bad_content_format_text, json_req_mail_mpss_bad_natural_legal, \
    json_req_mail_mpss_documentHash_without_hashAlgorithm, json_req_mail_mpss_documentHash_without_hash, \
    json_req_mail_mpss_documentHash_with_bad_hash, json_req_mail_mpss_documentHash_bad_hashAlgorithm, \
    json_req_mail_mpss_bad_from, json_req_mail_mpss_bad_from_mpss, json_req_mail_mpss_bad_whitelist_to, \
    json_req_mail_mpss_bad_whitelist_cc, json_req_mail_mpss_bad_whitelist_bcc


# negativní testy
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_bad_format_language])
def test_mail_negative_format_language(ns_url, auth_params, auth, json_data):
    """negativní test pro test jazyka a formatu"""
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
    assert resp.status_code == 400
    #error_message = resp.json()['errors']['319'][0]
    #assert 'Allowed values for Language: cs,en.' in error_message
    error_message = resp.json()['errors']['329'][0]
    assert 'Allowed values for Language: cs, en.' in error_message


@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_bad_11_attachments, json_req_mail_kb_bad_11_attachments])
def test_mail_negative_11_attachments(ns_url, auth_params, auth, json_data):
    """max priloh klady test, ale MCS zařízne"""
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
    assert resp.status_code == 400
    error_message = resp.json()['errors']['361'][0]
    assert 'Maximum count of Attachments is 10' in error_message


# TODO: dodelat assrty pro errors == zatím není vyvinuta hláška  bude vyvinuto v drobné změny 3
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data, expected_result", [
    (json_req_mail_bad_identifier_mpss_basic, {'Identifier.Identity': ['The Identity field is required.'], 'Identifier.IdentityScheme': ['The IdentityScheme field is required.']}),
    (json_req_mail_bad_identifier_scheme_mpss_basic, {'Identifier.IdentityScheme': ['The IdentityScheme field is required.']}),
    (json_req_mail_bad_identifier_identity_mpss_basic, {'Identifier.Identity': ['The Identity field is required.']}),
])
def test_mail_negative_identifier_request(auth_params, auth, json_data, ns_url, expected_result):
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
    assert resp['status'] == 400
    print(resp)
    assert resp['errors'] == expected_result, f'Expected {expected_result}, but got {resp["errors"]}'


@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_bad_content_format_text])
def test_mail_negative_content_format(ns_url, auth_params, auth, json_data):
    """negativní test pro test jazyka a formatu"""
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
    assert resp.status_code == 400
    error_message = resp.json()['errors']['327'][0]
    assert 'Allowed values for Format: application/html, application/mht, application/text, html, text/html, text/plain.' in error_message


# pro testy zabezpeceni, jake sms jsou mozne odespilat pres urcite uzivatele - pouzita vnorena parametrizace
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST", "XX_KBINSG_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_basic_legal])
def test_mail_bad_auth(auth_params, auth, json_data, ns_url):
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
    assert resp.status_code == 403


@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_bad_natural_legal])
def test_mail_bad_name(auth_params, auth, json_data, ns_url):
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
    assert resp['status'] == 400
    print(resp)
    errors = resp.get('errors', {})
    assert any('FirstName required' in error for error_list in errors.values() for error in error_list)
    assert any('Surname required' in error for error_list in errors.values() for error in error_list)
    assert any('Name required' in error for error_list in errors.values() for error in error_list)


@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data, expected_error", [
    (json_bad_req_mail_mpss_empty_party_from, {
        '320': ['Party must contain either LegalPerson or NaturalPerson.']
    }),
    (json_bad_req_mail_mpss_both_party_from, {
        '320': ['Party must contain either LegalPerson or NaturalPerson.']
    })
])
def test_mail_bad_party(auth_params, auth, json_data, ns_url, expected_error):
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
    assert resp['status'] == 400
    print(resp)
    result_error = resp.get('errors', {})
    assert result_error == expected_error


@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data, expected_error", [
    (json_req_mail_mpss_documentHash_without_hashAlgorithm, {
        'DocumentHash.HashAlgorithm': ['The HashAlgorithm field is required.']}),
    (json_req_mail_mpss_documentHash_without_hash, {
        'DocumentHash.Hash': ['The Hash field is required.']}),
    (json_req_mail_mpss_documentHash_with_bad_hash, {
        '310': ['Invalid Hash.']}),
    (json_req_mail_mpss_documentHash_bad_hashAlgorithm, {
        '0': ["Invalid HashAlgorithm = 'MMI-1989'. Allowed HashAlgorithms: SHA-256, "
       'SHA-384, SHA-512, SHA-3']
    })
])
def test_mail_negative_documentHash(ns_url, auth_params, auth, json_data, expected_error):
    """negativní test pro test jazyka a formatu"""
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
    assert resp.status_code == 400
    print(resp)
    result_error = resp.json()['errors']
    assert result_error == expected_error


@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_bad_from,
                                       json_req_mail_mpss_bad_from_mpss])
def test_mail_negative_from(ns_url, auth_params, auth, json_data):
    """negativní test pro test jazyka a formatu"""
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
    assert resp.status_code == 400
    error_message = resp.json()['errors']['PredicateValidator'][0]
    assert 'Allowed domain names for sender: kb.cz, kbsluzby.cz, kbinfo.cz, mpss.cz, mpss-info.cz.' in error_message


@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_bad_whitelist_to,
                                       json_req_mail_mpss_bad_whitelist_cc,
                                       json_req_mail_mpss_bad_whitelist_bcc])
def test_mail_negative_to_cc_bcc(ns_url, auth_params, auth, json_data):
    """negativní test pro test jazyka a formatu"""
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

    session = requests.session()
    resp = session.get(
        URLS[url_name] + f"/v1/notification/result/{notification_id}",
        json=json_data,
        auth=(username, password),
        verify=False
    )
    assert resp.status_code == 200
    error_code = resp.json()['errors'][0]['code']
    error_message = resp.json()['errors'][0]['message']
    assert error_code == "SMTP-WHITELIST-EXCEPTION"
    assert 'Could not send MPSS email to recipient outside the whitelist: marek.mikel@gmail.com' in error_message
