import pytest

from response.codebooks_json import json_resp_academic_degrees_after, json_resp_paymentDays, \
    json_resp_academic_degrees_before, json_resp_bank_codes, json_resp_case_states, \
    json_resp_classification_of_economic_activities, json_resp_contact_types, json_resp_countries, \
    json_resp_country_code_phone_idc, json_resp_currencies, json_resp_customer_roles, json_resp_developers, \
    json_resp_developer_projects, json_resp_document_on_sa_types, json_resp_drawing_durations, json_resp_drawing_types, \
    json_resp_ea_codes_main, json_resp_education_levels, json_resp_employment_types
from Tests.backend.pytest.tests.noby_rest.models.enum import Codebooks
from Tests.backend.pytest.tests.noby_rest.construct_api.Codebooks.get_all import get_all_codebooks


#TODO: assert pro CIS ciselnik vs. nas(hotovo)
@pytest.mark.parametrize("codebook_name, expected_resp_json", [
    (
            Codebooks.PAYMENTDAYS,
            json_resp_paymentDays
    ),
    (
            Codebooks.ACADEMICDEGREESAFTER,
            json_resp_academic_degrees_after
    ),
    (
            Codebooks.ACADEMICDEGREESBEFORE,
            json_resp_academic_degrees_before
    ),
    (
            Codebooks.BANKCODES,
            json_resp_bank_codes
    ),
    (
            Codebooks.CLASSIFICATIONOFECONOMICACTIVITIES,
            json_resp_classification_of_economic_activities
    ),
    (
            Codebooks.CASESTATES,
            json_resp_case_states
    ),
    (
            Codebooks.CONTACTTYPES,
            json_resp_contact_types
    ),
    (
            Codebooks.COUNTRIES,
            json_resp_countries
    ),
    (
            Codebooks.CURRENCIES,
            json_resp_currencies
    ),
    (
            Codebooks.CUSTOMERROLES,
            json_resp_customer_roles
    ),
    (
            Codebooks.DEVELOPERS,
            json_resp_developers
    ),
    (
            Codebooks.DEVELOPERPROJECTS,
            json_resp_developer_projects
    ),
    (
            Codebooks.DOCUMENTONSATYPES,
            json_resp_document_on_sa_types
    ),
    (
            Codebooks.DRAWINGDURATIONS,
            json_resp_drawing_durations
    ),
    (
            Codebooks.DRAWINGTYPES,
            json_resp_drawing_types
    ),
    (
            Codebooks.EACODESMAIN,
            json_resp_ea_codes_main
    ),
    (
            Codebooks.EDUCATIONLEVELS,
            json_resp_education_levels
    ),
    (
            Codebooks.EMPLOYMENTTYPES,
            json_resp_employment_types
    ),
]
)
def test_e2e_get_codebooks(codebook_name, expected_resp_json):
    resp = get_all_codebooks(codebook_name)

    assert resp[0] == 200
    assert resp[1] == codebook_name
    assert resp[2] == expected_resp_json
