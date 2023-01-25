import pytest

from Tests.backend.pytest.tests.noby_rest.models.enum import Codebooks_all, Codebooks, ProductTypes
from Tests.backend.pytest.tests.noby_rest.construct_api.Codebooks.get_all import get_all_codebooks, get_codebooks, \
    get_codebooks_fixation_period_length


@pytest.mark.parametrize("codebook_name", [
    Codebooks_all.ACADEMICDEGREESAFTER,
    Codebooks_all.ACADEMICDEGREESBEFORE,
    Codebooks_all.BANKCODES,
    Codebooks_all.CASESTATES,
    Codebooks_all.CLASSIFICATIONOFECONOMICACTIVITIES,
    Codebooks_all.CONTACTTYPES,
    Codebooks_all.COUNTRIES,
    Codebooks_all.CURRENCIES,
    Codebooks_all.CUSTOMERPROFILES,
    Codebooks_all.CUSTOMERROLES,
    Codebooks_all.DEVELOPERS,
    Codebooks_all.DEVELOPERPROJECTS,
    Codebooks_all.DOCUMENTONSATYPES,
    Codebooks_all.DRAWINGDURATIONS,
    Codebooks_all.DRAWINGTYPES,
    Codebooks_all.EACODESMAIN,
    Codebooks_all.EDUCATIONLEVELS,
    Codebooks_all.EMPLOYMENTTYPES,
    Codebooks_all.FEES,
    Codebooks_all.FIXEDRATEPERIODS,
    Codebooks_all.FORMTYPES,
    Codebooks_all.GENDERS,
    Codebooks_all.HOUSEHOLDTYPES,
    Codebooks_all.IDENTIFICATIONDOCUMENTTYPES,
    Codebooks_all.INCOMEMAINTYPES,
    Codebooks_all.INCOMEFOREIGNTYPES,
    Codebooks_all.INCOMEOTHERTYPES,
    Codebooks_all.JOBTYPES,
    # Codebooks.LEGALCAPACITIES,
    Codebooks_all.LOANPURPOSES,
    Codebooks_all.LOANKINDS,
    Codebooks_all.LOANINTERESTRATEANNOUNCEDTYPES,
    Codebooks_all.MANDANTS,
    Codebooks_all.MARITALSTATUSES,
    Codebooks_all.OBLIGATIONCORRECTIONTYPES,
    Codebooks_all.OBLIGATIONTYPES,
    Codebooks_all.PAYMENTDAYS,
    Codebooks_all.PAYOUTTYPES,
    Codebooks_all.POSTCODES,
    Codebooks_all.PRODUCTTYPES,
    Codebooks_all.PROPERTYSETTLEMENTS,
    Codebooks_all.REALESTATETYPES,
    Codebooks_all.REALESTATEPURCHASETYPES,
    Codebooks_all.SALESARRANGEMENTSTATES,
    Codebooks_all.SALESARRANGEMENTTYPES,
    Codebooks_all.SIGNATURETYPES,
    Codebooks_all.WORKFLOWTASKCATEGORIES,
    Codebooks_all.WORKFLOWTASKSTATES,
    Codebooks_all.WORKFLOWTASKTYPES,
    Codebooks_all.WORKSECTORS

]
                         )
def test_get_all_codebooks(codebook_name):
    resp = get_all_codebooks(codebook_name)
    assert resp[0] == 200

    check = resp[2][1]

    s = 'isValid' in check
    print(s, codebook_name)


@pytest.mark.parametrize("codebook_name, product_type", [
    (Codebooks.PRODUCTLOANKINDS, ProductTypes.MORTGAGE),
    (Codebooks.FIXEDRATEPERIODS, ProductTypes.MORTGAGE)]
                         )
def test_get_codebooks_fixed_rate_periods(codebook_name, product_type):
    resp = get_codebooks(codebook_name, product_type)
    assert resp[0] == 200

    check = resp[1][0]

    s = 'isValid' in check
    print(s, codebook_name)


@pytest.mark.parametrize("product_type", [
    ProductTypes.MORTGAGE]
                         )
def test_get_codebooks_fixation_period_length(product_type):
    resp = get_codebooks_fixation_period_length(product_type)
    assert resp[0] == 200
