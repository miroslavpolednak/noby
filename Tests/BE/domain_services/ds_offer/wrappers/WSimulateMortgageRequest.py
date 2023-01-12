from .WBase import WBase

from Mortgage.SimulateMortgage_pb2 import SimulateMortgageRequest

class WSimulateMortgageRequest(WBase):
    def __init__(self, js_dict: dict = None):
        super().__init__(stub=SimulateMortgageRequest(), js_dict = js_dict)

# message SimulateMortgageRequest {
# 	string ResourceProcessId = 1;
# 	BasicParameters BasicParameters = 2;
# 	MortgageSimulationInputs SimulationInputs = 3;
# }