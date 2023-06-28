import pytest

from typing import List

from fe_api.FeAPI import FeAPI
from fe_api.enums import ECodebook

codebooks: List[pytest.param] = [pytest.param(e, id=e.value) for e in ECodebook]
@pytest.mark.parametrize("c", codebooks)
def test_codebook(c: ECodebook):
    """Test API get codebook."""
    items = FeAPI.Codebooks.get_codebook(c, False)
    assert isinstance(items, list)
    assert len(items) > 0
    print(f'- codebook: {c} [{len(items)}]')
