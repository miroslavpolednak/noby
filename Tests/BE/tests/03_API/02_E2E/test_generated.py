import pytest

from typing import List

from common import ETestLayer, ETestType
from DATA import TestDataProvider, TestDataRecord, ESource

__SOURCE = ESource.Generator
__LAYER = ETestLayer.API
__TYPE = ETestType.COMP_CASE

#TODO: add method to TestDataProvider getRecordsByEnvForApi
records: List[TestDataRecord] = TestDataProvider.load_records(source=__SOURCE, layers=__LAYER, types=__TYPE)

records_by_order = dict()
for r in records:
    records_by_order[r.order] = r
orders: List[int] = list(records_by_order.keys())

def get_label(order: int):
    if order not in orders:
        return ' No Data Found '
    return records_by_order[order].test_label

@pytest.mark.parametrize("order", orders, ids=get_label)
#def test_generated_E2E(order: int):
def test(order: int):
    """Test API E2E generated."""
    data_json =  TestDataProvider.load_record_data(__SOURCE, order)
    assert data_json is not None
    #TODO:
