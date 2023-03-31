from ._Base import Base

class Household(Base):

    def __init__(self):
        super().__init__(route='household')

    def get_household_list(self, sales_arrangement_id: int) -> dict:
        return self.get(f'list/{sales_arrangement_id}')

    def get_household(self, household_id: int) -> dict:
        return self.get(str(household_id))

    def create_household(self, request: dict) -> dict:
        return self.post('', request)

    def delete_household(self, household_id: int) -> dict:
        return self.delete(str(household_id))

    def set_household_parameters(self, household_id: int, request: dict) -> dict:
        return self.put(str(household_id), request)

    def set_household_customers(self, household_id: int, request: dict) -> dict:
        return self.put(f'{household_id}/customers', request)
