from typing import List
from wrappers import WSimulateMortgageRequest

class ITransformer:
    def transform(self) -> List[WSimulateMortgageRequest]:
        """Load source data and creates simulation requests."""
        pass
