# ----------------------------
import _setup
# ----------------------------



# --------------------------------------------------------
# GET DS URLs
# --------------------------------------------------------
from typing import List
from common import config, EService, EServiceType

cfg_is_discovery_db_connection = config.is_discovery_db_connection
cfg_env_name = config.env_name
cfg_server = config.server

for s in EService:
    url = config.get_service_url(s, EServiceType.GRPC)
    print(url)
# --------------------------------------------------------