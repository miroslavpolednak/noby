from enum import Flag, auto

class ETestEnvironment(Flag):
    DEV = auto()
    FAT = auto()
    SIT1 = auto()
    UAT = auto()
