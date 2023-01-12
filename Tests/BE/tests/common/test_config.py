"""Test config."""

import pytest
from typing import List
from common import config, EService, EServiceType

def test_config_env():
    """Test config env_name."""
    assert config.env_name
    assert config.server

def test_config_discovery():
    """Test config is_discovery_db_connection."""
    assert config.is_discovery_db_connection

services: List[pytest.param] = [pytest.param(e, id=e.value) for e in EService]
@pytest.mark.parametrize("s", services)
def test_config_service_grpc_urls(s: EService):
    """Test config grpc urls of services."""
    url = config.get_service_url(s, EServiceType.GRPC)
    assert url is not None, f'GRPC URL not found [service: {s}]'
    print(f'- port: {url[-5:]};')

