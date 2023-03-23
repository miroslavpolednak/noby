# ----------------------------
import _setup
# ----------------------------

from common import EnumExtensions
from fe_api.FeAPI import FeAPI
from fe_api.enums import ECodebook

codebooks = EnumExtensions.enum_to_list(ECodebook)

# --------------------------------------------------------
# PROCESS
# --------------------------------------------------------

for c in codebooks:
    items = FeAPI.Codebooks.get_codebook(c, False)
    assert isinstance(items, list)
    assert len(items) > 0
    print(f'- codebook: {c} [{len(items)}]')