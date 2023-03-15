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

generated: List[TestDataEntry] = [
    TestDataEntry(1, dict(label = 'Generated A', offer = dict()), Options(ETestEnvironment.DEV | ETestEnvironment.FAT | ETestEnvironment.SIT1, ETestLayer.DS | ETestLayer.WEB, ETestType.E2E)),
    TestDataEntry(2, dict(label = 'Generated B', offer = dict(), case = dict()), Options(ETestEnvironment.DEV | ETestEnvironment.UAT, ETestLayer.API | ETestLayer.WEB, ETestType.COMP_OFFER)),
    TestDataEntry(3, dict(label = 'Generated C', offer = dict(), case = dict()), Options(ETestEnvironment.FAT | ETestEnvironment.UAT, ETestLayer.DS | ETestLayer.API, ETestType.COMP_CASE)),
]

TestDataProvider.reset()
TestDataProvider.import_custom()
TestDataProvider.import_generated(datetime.now(), generated)

filter: Options = Options.from_json(dict())     # all
print('Filter: ', filter)

records = TestDataProvider.load_records(source = ESource.Generator, environments = ETestEnvironment.DEV, types = ETestType.COMP_OFFER)
for r in records:
    print(r)

# --------------------------------------------------------