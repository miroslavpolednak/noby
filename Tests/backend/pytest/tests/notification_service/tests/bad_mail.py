import itertools

import pytest
import requests
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
from ..conftest import URLS
from ..json.request.mail_kb_json import json_req_mail_kb_bad_11_attachments, json_req_mail_kb_basic_legal
from ..json.request.mail_mpss_json import json_req_mail_mpss_basic_legal
from ..json.request.mail_mpss_neg_json import json_bad_req_mail_mpss_empty_party_from, json_bad_req_mail_mpss_both_party_from, \
    json_req_mail_mpss_negative_basic_format_text_plain, json_req_mail_mpss_bad_11_attachments, \
    json_req_mail_bad_identifier_mpss_basic, json_req_mail_bad_identifier_identity_mpss_basic, \
    json_req_mail_bad_identifier_scheme_mpss_basic, json_req_mail_mpss_bad_format_language, \
    json_req_mail_mpss_bad_content_format_text,  json_req_mail_mpss_bad_natural_legal


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
    error_message = resp.json()['errors']['324'][0]
    assert 'Allowed values for Language: cs,en.' in error_message


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


# TODO: dodelat assrty pro errors hlášku, bude vyvinuto v drobné změny 3
@pytest.mark.parametrize("auth", ["XX_EPSY_RMT_USR_TEST"], indirect=True)
@pytest.mark.parametrize("json_data", [json_req_mail_mpss_bad_content_format_text,
                                       json_req_mail_mpss_negative_basic_format_text_plain])
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
    error_message = resp.json()['errors']['322'][0]
    assert 'Allowed values for Format: application/html,application/mht,html,text/html.' in error_message


# pro testy zabezpeceni, jake sms jsou mozne odespilat pres urcite uzivatele - pouzita vnorena parametrizace
@pytest.mark.parametrize("auth", ["XX_INSG_RMT_USR_TEST", "XX_NOBY_RMT_USR_TEST"], indirect=True)
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
        '314': ['Party must contain either LegalPerson or NaturalPerson.']
    }),
    (json_bad_req_mail_mpss_both_party_from, {
        '314': ['Party must contain either LegalPerson or NaturalPerson.']
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