from Tests.backend.pytest.tests.noby_rest.construct_api.uc_modelace_hypoteky.get_offer_mortgage_offerid import get_offer
import json


def json_req_create_case(mortgage): return {
    "offerId": get_offer(mortgage),
    "firstName": "JANA",
    "lastName": "NOVÁKOVÁ",
    "dateOfBirth": "1981-01-01T00:00:00",
    "phoneNumberForOffer": "+420 777678678",
    "emailForOffer": "novakova@testcm.cz",
    "identity": {
        "id": 951070696, "scheme": 2}
}
