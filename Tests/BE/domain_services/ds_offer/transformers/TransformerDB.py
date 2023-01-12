from typing import List
from .ITransformer import ITransformer

from wrappers import WSimulateMortgageRequest

class TransformerDB(ITransformer):

    def __init__(self, source_path: str):
        assert source_path is not None, f'Source must be provided!'
        self._source_path = source_path


    def transform(self) -> List[WSimulateMortgageRequest]:
        """Load source data and creates simulation requests."""
        return []
