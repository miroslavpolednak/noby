# ----------------------------
import _setup
# ----------------------------

import datetime
from DATA import TestDataProvider, TestDataRecord
from common import config, ETestType, LogFileContext, LogFolderContext
from E2E import ApiProcessor, ApiReaderOffer, ApiReaderCase


# --------------------------------------------------------
# PREPARE DATA
# --------------------------------------------------------

# reset DB, import data
TestDataProvider.reset()
TestDataProvider.import_custom()
# TestDataProvider.import_generated(datetime.now(), generated)


# --------------------------------------------------------
# PROCESS
# --------------------------------------------------------

# load records
records = TestDataProvider.load_records_custom_api(ETestType.E2E)

records = [records[-1]] # last item

def process_record(record_to_test: TestDataRecord):

    data_json = TestDataProvider.load_record_data(record_to_test.source, record_to_test.order)
    print(data_json)

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

def process():
    #with LogFolderContext():
        for record_to_test in records:
            with LogFileContext(record_to_test.log_file_name) as log_file_context:
                process_record(record_to_test)

process()

#TODO: parametrize method 'process' (use contexts)