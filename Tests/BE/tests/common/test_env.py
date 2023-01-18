"""Test environment."""

import os

def test_env_variables_core():
    """Test core env variables."""
    assert os.getenv("SERVER")
    assert os.getenv("ENV")

def test_env_variables_fe_api():
    """Test FeAPI env variables."""
    assert os.getenv("FE_API_URL")

def test_env_variables_IS_discovery():
    """Test ServiceDiscovery env variables."""
    assert os.getenv("IS_DISCOVERY_DB_SERVER")
    assert os.getenv("IS_DISCOVERY_DB_DATABASE")
    assert os.getenv("IS_DISCOVERY_DB_USER")
    assert os.getenv("IS_DISCOVERY_DB_PASSWORD")

