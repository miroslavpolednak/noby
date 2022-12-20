import pytest

from response.codebooks_json import json_resp_academic_degrees_after, json_resp_paymentDays, \
    json_resp_academic_degrees_before, json_resp_bank_codes, json_resp_case_states
from Tests.backend.pytest.tests.noby_rest.models.enum import Codebooks
from Tests.backend.pytest.tests.noby_rest.construct_api.Codebooks.get_all import get_all_codebooks


#TODO: dodelat vsechy codebooky ze swaggeru
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
            Codebooks.BANKCODE,
            json_resp_bank_codes
    ),
(
            Codebooks.CASESTATES,
            json_resp_case_states
    )
]
)
def test_get_codebooks(codebook_name, expected_resp_json):
    resp = get_all_codebooks(codebook_name)
    print(resp)

    assert resp[0] == 200
    assert resp[1] == codebook_name
    assert resp[2] == expected_resp_json
