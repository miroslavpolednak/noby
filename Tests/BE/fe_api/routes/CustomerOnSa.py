from ._Base import Base

class CustomerOnSa(Base):

    def __init__(self):
        super().__init__(route='customer-on-sa')

    def get_obligation(self, customer_on_sa_id: int, obligation_id: int) -> dict:
        return self.get(f'{customer_on_sa_id}/obligation/{obligation_id}')

    def get_income(self, customer_on_sa_id: int, income_id: int) -> dict:
        return self.get(f'{customer_on_sa_id}/income/{income_id}')

    def create_income(self, customer_on_sa_id: int, request: dict) -> dict:
        return self.post(f'{customer_on_sa_id}/income', request)

    def create_obligation(self, customer_on_sa_id: int, request: dict) -> dict:
        return self.post(f'{customer_on_sa_id}/obligation', request)
