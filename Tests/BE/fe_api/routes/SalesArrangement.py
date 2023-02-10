from ._Base import Base

class SalesArrangement(Base):

    def __init__(self):
        super().__init__(route='sales-arrangement')

    def get_customers(self, sales_arrangement_id: int) -> dict:
        return self.get(f'{sales_arrangement_id}/customers')
