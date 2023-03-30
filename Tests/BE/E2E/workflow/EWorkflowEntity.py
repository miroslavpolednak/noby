from enum import Enum

class EWorkflowEntity(Enum):
    CASE_HOUSEHOLDS = r'^\bcase.households\b(\[[0-2]\])?$'
    CASE_HOUSEHOLDS_CUSTOMER = r'^\bcase.households\b\[[0-2]\]\.customer[1-2]$'
    CASE_HOUSEHOLDS_CUSTOMER_INCOMES = r'^\bcase.households\b\[[0-2]\]\.customer[1-2]\.incomes(\[\d{1,3}\])?$'
    CASE_HOUSEHOLDS_CUSTOMER_OBLIGATIONS = r'^\bcase.households\b\[[0-2]\]\.customer[1-2]\.obligations(\[\d{1,3}\])?$'
    CASE_PARAMETERS = r'^\bcase.parameters\b$'
    CASE_PARAMETERS_LOANREALESTATES = r'^\bcase.parameters\b\.loanRealEstates(\[\d{1,3}\])?$'