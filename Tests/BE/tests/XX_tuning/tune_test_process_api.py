# ----------------------------
import _setup
# ----------------------------

from DATA import TestDataProvider
from common import config, ETestType
from E2E import ApiProcessor, ApiReaderOffer, ApiReaderCase


# --------------------------------------------------------
# PREPARE DATA
# --------------------------------------------------------

# reset DB, import data
TestDataProvider.reset()
TestDataProvider.import_custom()


# --------------------------------------------------------
# LOAD DATA
# --------------------------------------------------------

# load records
records = TestDataProvider.load_records_custom_api(ETestType.E2E)

index = 1

if (len(records) <= index):
    print(f'RECORD WITH INDEX {index} NOT FOUND [total records: {len(records)}] !!!')
    quit()

# load data json
data_json = TestDataProvider.load_record_data(records[index].source, records[index].order)
print(data_json)


# --------------------------------------------------------
# PROCESS
# --------------------------------------------------------

result = ApiProcessor(data_json).process()

if isinstance(result, Exception):
    print('Error: ', result)
    quit()

print('Result: ', result)

if 'case_id' in result.keys():
    case_id = result['case_id']
    case = ApiReaderCase().load(case_id)
    case_json = case.to_json_value()
    print(f'https://{config.environment.name.lower()}.noby.cz/undefined#/mortgage/case-detail/{case_id}')

#https://dev.noby.cz/undefined#/mortgage/case-detail/3014728
#https://dev.noby.cz/undefined#/mortgage/case-detail/3014730

# ApiWriterCase_2194675515920 | create_customers.res [[{'errorCode': 90001, 'message': 'Status(StatusCode="Internal", Detail="HouseholdTypeId 128 does not exist. (Parameter \'householdTypeId\')")', 'description': None, 'severity': 1}]]