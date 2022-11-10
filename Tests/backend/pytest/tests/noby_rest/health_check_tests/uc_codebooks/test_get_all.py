import pytest

from Tests.backend.pytest.tests.noby_rest.models.enum import Codebooks
from Tests.backend.pytest.tests.noby_rest.construct_api.Codebooks.get_all import get_all_codebooks


@pytest.mark.parametrize("codebook_name", [
    Codebooks.ACADEMICDEGREESAFTER,
    Codebooks.ACADEMICDEGREESBEFORE,
    Codebooks.ACTIONCODESSAVINGS,
    Codebooks.ACTIONCODESSAVINGSLOAN,
    Codebooks.CASESTATES,
    Codebooks.CLASSIFICATIONOFECONOMICACTIVITIES,
    Codebooks.CONTACTTYPES,
    Codebooks.COUNTRIES,
    Codebooks.CURRENCIES,
    Codebooks.CUSTOMERROLES,
    Codebooks.DEVELOPERS,
    Codebooks.DEVELOPERPROJECTS,
    Codebooks.EDUCATIONLEVELS,
    Codebooks.EMPLOYMENTTYPES,
    Codebooks.FEES,
    Codebooks.FIXEDRATEPERIODS,
    Codebooks.GENDERS,
    Codebooks.IDENTIFICATIONDOCUMENTTYPES,
    Codebooks.INCOMEMAINTYPES,
    Codebooks.INCOMEFOREIGNTYPES,
    Codebooks.INCOMEOTHERTYPES,
    Codebooks.JOBTYPES,
    Codebooks.LEGALCAPACITIES,
    Codebooks.LOANPURPOSES,
    Codebooks.LOANKINDS,
    Codebooks.MANDANTS,
    Codebooks.MARITALSTATUSES,
    Codebooks.OBLIGATIONTYPES,
    Codebooks.PAYMENTDAYS,
    Codebooks.POSTCODES,
    Codebooks.PRODUCTTYPES,
    Codebooks.PROPERTYSETTLEMENTS,
    Codebooks.REALESTATETYPES,
    Codebooks.REALESTATEPURCHASETYPES,
    Codebooks.SALESARRANGEMENTSTATES,
    Codebooks.SALESARRANGEMENTTYPES,
    Codebooks.SIGNATURETYPES,
    Codebooks.WORKFLOWTASKCATEGORIES,
    Codebooks.WORKFLOWTASKSTATES,
    Codebooks.WORKFLOWTASKTYPES,
    Codebooks.WORKSECTORS

]
)
def test_get_all_codebooks(get_all_codebooks, get_cookies, webapi_url, codebook_name):
    resp = get_all_codebooks
    assert resp[0] == 200
