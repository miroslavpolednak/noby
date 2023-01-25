from ._Base import Base

class Household(Base):

    def __init__(self):
        super().__init__(route='household')

    def create_household(self, request: dict) -> dict:
        return self.post('', request)

    def set_household_parameters(self, household_id: int, request: dict) -> dict:
        return self.put(str(household_id), request)
