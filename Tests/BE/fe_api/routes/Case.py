from ._Base import Base

class Case(Base):

    def __init__(self):
        super().__init__(route='case')

    def get_case(self, case_id: int) -> dict:
        return self.get(str(case_id))

# GET https://fat.noby.cz/api/case/3014499
# {"caseOwner":{"cpm":"990614w","icp":""},"caseId":3014499,"firstName":"Janek","lastName":"Ledecký","dateOfBirth":"1965-03-21T00:00:00","state":1,"stateName":"Příprava žádosti","contractNumber":"","targetAmount":1000000,"productName":"Hypoteční úvěr","createdTime":"2023-03-03T11:27:09.67","createdBy":"Filip Tůma","stateUpdated":"2023-03-03T11:27:09.667","emailForOffer":"j.l@jl","phoneNumberForOffer":"+420 111555999"}