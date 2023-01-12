import pytest

from domain_services.ds_offer.wrappers import *
from domain_services.ds_offer.OfferSvc import OfferSvc

from domain_services.ds_offer import db_provider, fs_provider

service = OfferSvc()

def test_service():
    """Test service."""
    assert 1==1

def test_get_offer():
    """Test service get offer."""
    offer = service.get_offer(1)
    assert offer is not None
    assert offer.OfferId == 1

def test_get_mortgage_offer():
    """Test service get mortgage offer."""
    offer = service.get_mortgage_offer(1)
    assert offer is not None
    assert offer.OfferId == 1

offer_ids = list(range(1400, 1480, 5))
offer_ids = list(range(400, 450, 5))
#@pytest.mark.parametrize("id", offer_ids, ids=lambda id: f'OfferId: {id}')
@pytest.mark.parametrize("id", offer_ids, ids=lambda id: id)
def test_get_mortgage_offer_detail(id: int):
    """Test service get mortgage detail."""
    offer = service.get_mortgage_offer_detail(id)
    #print(offer)
    assert offer is not None
    assert offer.OfferId == id

# def test_get_mortgage_offer_full_payment_schedule():
#     """Test service get offer."""
#     schedule = service.get_mortgage_offer_full_payment_schedule(1)
#     assert schedule is not None


# -----------------------------------------------------------------
#   load from DB
# -----------------------------------------------------------------
    # db_provider.prepare_input_data()
    # bp = WBasicParameters().load_from_db(1)
    # print(bp)
# -----------------------------------------------------------------

json_files = fs_provider.load()[0:1]
simulation_requests = list(map(lambda f: WSimulateMortgageRequest(f['json_dict']), json_files))

@pytest.mark.parametrize("req", simulation_requests, ids=lambda r: f'{str(r.js_dict)[0:250]} ... ')
def test_simulate(req: WSimulateMortgageRequest):
    """Test service simulate mortgage."""
    #print(req.stub)
    resp = service.simulate_mortgage(req.stub)
    assert resp is not None
    assert resp.OfferId is not None
    print(f'- OfferId: {resp.OfferId};')
