import pytest

from Tests.backend.pytest.tests.noby_rest.models.enum import Codebooks_all, Codebooks, ProductTypes
from Tests.backend.pytest.tests.noby_rest.construct_api.Codebooks.get_all import get_all_codebooks, get_codebooks, \
    get_codebooks_fixation_period_length


# Přidáme všechny číselníky jako parametry testu s výjimkou Codebooks.LEGALCAPACITIES
@pytest.mark.parametrize("codebook_name", [getattr(Codebooks_all, attr) for attr in dir(Codebooks_all) if not attr.startswith('__') and attr != 'LEGALCAPACITIES' and attr != 'MARKETINGACTIONS'])
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
    check = resp[1][0]

    s = 'isValid' in check
    print(s)

