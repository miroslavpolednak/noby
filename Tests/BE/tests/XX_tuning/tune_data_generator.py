# ----------------------------
import _setup
# ----------------------------


# --------------------------------------------------------
# TEST DATA GENERATOR
# --------------------------------------------------------
import json
from DATA import TestDataProvider
from testdatagenerator.datagenerator import process
from testdatagenerator.offergenerator import process as process_offer

TestDataProvider.reset()
process()
process_offer()

# --------------------------------------------------------

records = TestDataProvider.load_records(source = None, environments = None, layers = None, types = None)

for r in records:
    print('RECORD ---> ', r)
    print('LABEL  ---> ', r.test_label)

# load data of records:
for r in records:
    data_json = TestDataProvider.load_record_data(r.source, r.order)
    print(json.dumps(data_json, indent = 4)[0:250])

# --------------------------------------------------------