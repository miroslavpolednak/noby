import json

from typing import List

from .Processing import Processing

from common import Log, config, DictExtensions
from DATA import JsonDataModificator

from business.offer import Offer
from business.case import Case

from .ApiWriterOffer import ApiWriterOffer
from .ApiWriterCase import ApiWriterCase
from .ApiReaderCase import ApiReaderCase

from .workflow.WorkflowStep import WorkflowStep
from .workflow.EWorkflowEntity import EWorkflowEntity

class ApiProcessor():

    def __init__(self, data_json: dict | str, snapshot_file_name: str = None):

        self.__log = Log.getLogger(f'{self.__class__.__name__}_{id(self)}')

        self.__snapshot_file_name = snapshot_file_name

        if isinstance(data_json, str):
            self.__offer_case_json = json.loads(data_json)
        elif isinstance(data_json, dict):
            self.__offer_case_json = data_json
        else:
            raise 'ApiProcessor.init - Invalid data json !'

        self.__api_writer_offer: ApiWriterOffer = None
        self.__api_writer_case: ApiWriterCase = None
        


    @staticmethod
    def process_list(offer_case_jsons: List[dict]) -> List[dict]:

        results: List[dict] = []

        for json in offer_case_jsons:
            r: dict = ApiProcessor(json).process()
            results.append(r)

        return results

    def process(self) -> dict | Exception:

        result: dict = None

        try:
            result = self.__process()
        except Exception as e:
            result = e

        return result

    


    def __process(self) -> dict:

        def handle_create_snapshot(case_id: int, order: int = None, workflow_step: WorkflowStep = None):
            print('handle_create_snapshot', case_id, workflow_step, order)

            assert case_id is not None
            
            # get JSON
            case = ApiReaderCase().load(case_id)
            case_json = case.to_json_value()
            case_json_str = DictExtensions.to_string(case_json)
            
            # build file name
            file_name = str(case_id) if self.__snapshot_file_name is None else self.__snapshot_file_name
            if order is not None:
                file_name += f'_{str(order).zfill(3)}'
            if workflow_step is not None:
                file_name += f'_{workflow_step.entity.name}_{workflow_step.type.name}'
            file_name += '.json'

            # save snapshot & log info
            Log.save_snapshot(file_name=file_name, json_string=case_json_str)
            self.__log.info(f'Snapshot [{file_name}]')

            
        #self.__process_workflow()

        offer_json: dict = Processing.get_key(self.__offer_case_json, 'offer')
        case_json: dict = Processing.get_key(self.__offer_case_json, 'case')
        
        # -------------------------------------------------
        # resolve modifications
        # -------------------------------------------------
        offer_json = JsonDataModificator.modify(offer_json)
        case_json = JsonDataModificator.modify(case_json)
        # -------------------------------------------------

        offer_id: int = None
        case_id: int = None

        if (offer_json is not None):
            offer = Offer.from_json(offer_json)
            self.__api_writer_offer = ApiWriterOffer(offer)
            build_result = self.__api_writer_offer.build()

            if isinstance(build_result, Exception):
                raise build_result
            
            offer_id = build_result
            # offer_link = f'https://{config.environment.name.lower()}.noby.cz/undefined#/mortgage/case-detail/{offer_id}'
            # self.__log.info(f'LINK to OFFER: {offer_link}')
            
        if (case_json is not None):
            case = Case.from_json(case_json)
            self.__api_writer_case = ApiWriterCase(case, offer_id, handle_create_snapshot=handle_create_snapshot)
            build_result = self.__api_writer_case.build()

            if isinstance(build_result, Exception):
                raise build_result
            
            case_id = build_result
            case_link = f'https://{config.environment.name.lower()}.noby.cz/undefined#/mortgage/case-detail/{case_id}'
            self.__log.info(f'LINK to CASE: {case_link}')

        self.__process_workflow()

        return dict(
            offer_id = offer_id,
            case_id = case_id,
        )


    def __process_workflow(self) -> dict:

        workflow_json: List[dict] = Processing.get_key(self.__offer_case_json, 'workflow')

        if (workflow_json is None):
            return None

        assert isinstance(workflow_json, list)

        workflow_steps: List[WorkflowStep] = list(map(lambda i: WorkflowStep(i), workflow_json))

        for step in workflow_steps:
            order = workflow_steps.index(step) + 1

            # log workflow
            log_message_details: str = f'path: {step.path}'
            if step.data is not None:
                log_message_details += f' , data: {step.data}'  
            log_message: str = f'WORKFLOW - {step.entity.name}.{step.type.name} [{log_message_details}]'
            self.__log.info(log_message)

            # exec workflow

            self.__api_writer_case.process_workflow_step(order, step)

    # def read_case(case_id) -> Case | Exception:

    #     result: Case | Exception = None

    #     try:
    #         result = ApiCaseLoader().load(case_id)
    #     except Exception as e:
    #         result = e

    #     return result


# ----------------------------------------------------------------------------------------------------------------------------------------------
# SCORING
# ----------------------------------------------------------------------------------------------------------------------------------------------
# GET https://fat.noby.cz/api/sales-arrangement/337/loan-application-assessment?newAssessmentRequired=true
# {"type":"https://tools.ietf.org/html/rfc7231#section-6.5.1","title":"SalesArrangement.RiskBusinessCaseId not defined.","status":400}


# ----------------------------------------------------------------------------------------------------------------------------------------------