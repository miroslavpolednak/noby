from ._Base import Base

class Offer(Base):

    def __init__(self):
        super().__init__(route='offer')

    def simulate_mortgage(self, request: dict) -> dict:
        return self.post('mortgage', request)

    def create_case(self, request: dict) -> dict:
        return self.post('mortgage/create-case', request)

