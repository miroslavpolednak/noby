from enum import Flag, auto

class EEnvironment(Flag):
    DEV = auto()
    FAT = auto()
    SIT = auto()
    UAT = auto()


# class Color(Flag):
#     RED = auto()
#     GREEN = auto()
#     BLUE = auto()
# purple = Color.RED | Color.BLUE
# white = Color.RED | Color.GREEN | Color.BLUE
# Color.GREEN in purple

# Build['debug']