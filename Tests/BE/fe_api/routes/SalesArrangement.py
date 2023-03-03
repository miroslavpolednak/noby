from ._Base import Base

class SalesArrangement(Base):

    def __init__(self):
        super().__init__(route='sales-arrangement')

    # https://wiki.kb.cz/display/HT/getSalesArrangementList
    def get_sales_arrangement_list(self, case_id: int) -> dict:
        return self.get(f'list/{case_id}')

    def get_sales_arrangement(self, sales_arrangement_id: int) -> dict:
        return self.get(str(sales_arrangement_id))

    def get_customers(self, sales_arrangement_id: int) -> dict: #TODO: Renemate to 'get_customer_list'
        return self.get(f'{sales_arrangement_id}/customers')

    def set_parameters(self, sales_arrangement_id: int, request: dict) -> dict:
        return self.put(f'{sales_arrangement_id}/parameters', request)
