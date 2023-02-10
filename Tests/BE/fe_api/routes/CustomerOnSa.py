from ._Base import Base

class CustomerOnSa(Base):

    def __init__(self):
        super().__init__(route='customer-on-sa')

    def create_income(self, customer_on_sa_id: int, request: dict) -> dict:
        return self.post(f'{customer_on_sa_id}/income', request)

    def create_obligation(self, customer_on_sa_id: int, request: dict) -> dict:
        return self.post(f'{customer_on_sa_id}/obligation', request)
