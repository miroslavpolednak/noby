import pytest

from typing import List

from common import ETestType, config
from DATA import TestDataProvider, TestDataRecord, ESource
from E2E import ApiProcessor, ApiReaderOffer, ApiReaderCase

records: List[TestDataRecord] = TestDataProvider.load_records_custom_api(ETestType.E2E)

records_by_order = dict()
for r in records:
    records_by_order[r.order] = r
orders: List[int] = list(records_by_order.keys())

def get_label(order: int):
    if order not in orders:
        return ' No Data Found '
    return records_by_order[order].test_label

@pytest.mark.parametrize("order", orders, ids=get_label)
def test(order: int):
    """Test API E2E custom."""

    # load data json
    data_json =  TestDataProvider.load_record_data(ESource.Custom, order)
    assert data_json is not None

    # process & check result
    result = ApiProcessor(data_json).process()
    assert isinstance(result, Exception) == False, result

    if 'case_id' in result.keys():
        case_id = result['case_id']

        # load case & check result
        case = ApiReaderCase().load(case_id)
        assert isinstance(case, Exception) == False, case

        # convert case to JSON & check result
        case_json = case.to_json_value()
        assert isinstance(case_json, dict), case

        print(f'https://{config.environment.name.lower()}.noby.cz/undefined#/mortgage/case-detail/{case_id}')


# ===================================================== warnings summary ====================================================== 
# DATA\model\TestDataRecord.py:5
#   D:\Users\992466q\source\repos\OneSolution\Tests\BE\DATA\model\TestDataRecord.py:5: PytestCollectionWarning: cannot collect test class 'TestDataRecord' because it has a __init__ constructor (from: tests/03_API/02_E2E/test_custom.py)
#     class TestDataRecord():

# DATA\model\TestDataRecord.py:5
#   D:\Users\992466q\source\repos\OneSolution\Tests\BE\DATA\model\TestDataRecord.py:5: PytestCollectionWarning: cannot collect test class 'TestDataRecord' because it has a __init__ constructor (from: tests/03_API/02_E2E/test_generated.py)
#     class TestDataRecord():