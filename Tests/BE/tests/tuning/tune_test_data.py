# ----------------------------
import _setup
# ----------------------------


# --------------------------------------------------------
# TEST DATA
# --------------------------------------------------------
from typing import List
from datetime import datetime
from DATA import TestDataProvider, TestDataEntry, Options, ESource
from common import ETestEnvironment, ETestLayer, ETestType

# {
#     "environments": ["DEV", "FAT", "SIT1", "UAT"],
#     "layers": ["DS", "API", "WEB"],#     "types": ["E2E", "COMP_OFFER", "COMP_CASE", "COMP_HOUSEHOLD"]
# }

# generated data example:
generated: List[TestDataEntry] = [
    TestDataEntry(1, dict(label = 'Generated A', offer = dict()), Options(ETestEnvironment.DEV | ETestEnvironment.FAT | ETestEnvironment.SIT1, ETestLayer.DS | ETestLayer.WEB, ETestType.E2E)),
    TestDataEntry(2, dict(label = 'Generated B', offer = dict(), case = dict()), Options(ETestEnvironment.DEV | ETestEnvironment.UAT, ETestLayer.API | ETestLayer.WEB, ETestType.COMP_OFFER)),
    TestDataEntry(3, dict(label = 'Generated C', offer = dict(), case = dict()), Options(ETestEnvironment.FAT | ETestEnvironment.UAT, ETestLayer.DS | ETestLayer.API, ETestType.COMP_CASE)),
]

# reset DB, import custom and generated data
TestDataProvider.reset()
TestDataProvider.import_custom()
TestDataProvider.import_generated(datetime.now(), generated)

# load data records by filter
f_source: ESource = ESource.Custom
f_environments: ETestEnvironment = ETestEnvironment.DEV
f_layers: ETestLayer = None # ETestLayer.API
f_types: ETestType = None # ETestType.COMP_OFFER

records = TestDataProvider.load_records(
        source = f_source, 
        environments = f_environments,
        layers = f_layers,
        types = f_types
    )

for r in records:
    print('RECORD ---> ', r)
    print('LABEL  ---> ', r.test_label)

# load data of records:
for r in records:
    data_json = TestDataProvider.load_record_data(r.source, r.order)
    print(data_json)

# --------------------------------------------------------