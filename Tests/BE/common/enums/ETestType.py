from enum import Flag, auto

class ETestType(Flag):
    E2E = auto()
    COMP_OFFER = auto()
    COMP_CASE = auto()
    COMP_HOUSEHOLD = auto()