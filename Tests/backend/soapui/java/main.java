import java.util.logging.Logger;

import org.json.JSONArray;
import org.json.JSONObject;
import java.util.HashMap;
import java.io.PrintWriter;

public class main
{
    private static final Logger log = Logger.getLogger(ReflectionCall.class.getName());

    //String host= "172.30.35.51";
    String host= "adpra191";
    int portBase=31000; //FAT


    //String host= "adpra193";
    //int portBase=33000; //UAT


    ReflectionCall callerServiceDiscovery;
    ReflectionCall callerCaseService;

    ReflectionCall callerCodebookService;
    ReflectionCall callerCustomerService;
    // TODO Document Service 5
    ReflectionCall callerOfferService;
    ReflectionCall callerProductService;
    // TODO Redirect Service 8
    ReflectionCall callerSalesArrangementService;
    ReflectionCall callerHouseholdService;
    ReflectionCall callerCustomerOnSAService;
    ReflectionCall callerUserService;




    //int portBase=32000; //SIT1
    // The format of the reflection method only supports package.service.method or package.service

    //gatTaskListHeaders.put("Correlation-Context","MpPartyId=3048");
    //gatTaskListHeaders.put("traceparent","00-ddc1760e36a462c9c03b2583b1c9a098-ea157dc423037e71-01");

    void init()
    {

        HashMap<String, String> emptyHeaders=new HashMap<String, String>();

        callerServiceDiscovery=new ReflectionCall(host , portBase+0, "CIS.InternalServices.ServiceDiscovery.v1.DiscoveryService","Basic YTph",3,emptyHeaders);
        callerCaseService=new ReflectionCall(host, portBase+1, "DomainServices.CaseService.v1.CaseService","Basic YTph",4,emptyHeaders);

        callerCodebookService=new ReflectionCall(host, portBase+3, "DomainServices.CodebookService","Basic YTph",3,emptyHeaders);
        callerCustomerService=new ReflectionCall(host, portBase+4, "DomainServices.CustomerService.V1.CustomerService","Basic YTph",3,emptyHeaders);
        // TODO Document Service 5
        callerOfferService=new ReflectionCall(host, portBase+6, "DomainServices.OfferService.v1.OfferService","Basic YTph",4,emptyHeaders);
        callerProductService=new ReflectionCall(host, portBase+7, "DomainServices.ProductService.v1.ProductService","Basic YTph",3,emptyHeaders);
        // TODO Redirect Service 8
        callerSalesArrangementService=new ReflectionCall(host, portBase+9, "DomainServices.SalesArrangementService.v1.SalesArrangementService","Basic YTph",4,emptyHeaders);
        callerHouseholdService=new ReflectionCall(host, portBase+9, "DomainServices.SalesArrangementService.v1.HouseholdService","Basic YTph",4,emptyHeaders);
        callerCustomerOnSAService=new ReflectionCall(host, portBase+9, "DomainServices.SalesArrangementService.v1.CustomerOnSAService","Basic YTph",4,emptyHeaders);
        callerUserService=new ReflectionCall(host, portBase+10, "DomainServices.UserService.v1.UserService","Basic YTph",3,emptyHeaders);
    }
/*
                    io.grpc.Metadata.Key<String> metaKey = io.grpc.Metadata.Key.of("Correlation-Context", Metadata.ASCII_STRING_MARSHALLER);
                    meta.put(metaKey,"MpPartyId=3048");
                    //io.grpc.Metadata.Key<String> t = io.grpc.Metadata.Key.of("traceparent", Metadata.ASCII_STRING_MARSHALLER);
                    //aut.put(t,"00-ddc1760e36a462c9c03b2583b1c9a098-ea157dc423037e71-01");
*/

    //TODO: pokud bude treba, tak podpora pro float
    public JSONObject toGrpcDecimal(int i)
    {
        return (new JSONObject()).put("units",i).put("nanos",0);
    }

    public static void main(String[] args) throws InterruptedException
    {
        System.out.println("Started (system out)");
        log.info("Started (system log.info)");
        System.out.println("Started (system out after log)");
        main me=new main();
        me.init();

        me.test_discovery();
        //me.test_validate_sa();
        //me.test_send_to_cmp();
        //me.test_get_offer();
        //me.test_list_sa();                          //  <---------------------
        //me.test_get_sa();
        //me.test_SearchCustomers();
        //me.getUser();
        //me.searchCustomer();
        //me.test_identitySchemesCodebook();
        //me.test_deleteCase();
        //me.test_updateCaseState();
        //me.test_caseTasks();
        //me.test_get_case();
        //me.test_adpra192();
        //me.test_e2e_json();
        //me.test_cm_instance();
    }


    void test_discovery()
    {
        JSONObject joA = new JSONObject();
        JSONObject joB = new JSONObject();
        JSONObject joC = new JSONObject();


        joA = new JSONObject();
        joB = new JSONObject();
        joC = new JSONObject();
        joA.put("Environment","FAT");
        joA.put("ServiceType","Grpc");
        log.info(joA.toString(2));
        joB=callerServiceDiscovery.call("GetServices", joA);
        log.info(joB.toString(2));    
    }


    void test_validate_sa()
    {

        JSONObject joA = new JSONObject();
        JSONObject joB = new JSONObject();
        JSONObject joC = new JSONObject();
        
        joA = new JSONObject();
        joA.put("SalesArrangementId","189");
        joB=callerSalesArrangementService.call("ValidateSalesArrangement", joA);
        log.info(joB.toString(2));        
    }


    void test_send_to_cmp()
    {

        JSONObject joA = new JSONObject();
        JSONObject joB = new JSONObject();
        JSONObject joC = new JSONObject();
        
        joA = new JSONObject();
        joA.put("SalesArrangementId","187");
        joB=callerSalesArrangementService.call("SendToCmp", joA);
        log.info(joB.toString(2));        
    }


    void test_get_offer()
    {

        JSONObject joA = new JSONObject();
        JSONObject joB = new JSONObject();
        JSONObject joC = new JSONObject();
        
        joA = new JSONObject();
        joA.put("OfferId","441");
        joB=callerOfferService.call("GetMortgageOffer", joA);
        log.info(joB.toString(2));        
    }


    void test_get_sa()
    {

        JSONObject joA = new JSONObject();
        JSONObject joB = new JSONObject();
        JSONObject joC = new JSONObject();
        
        //joA.put("SalesArrangementId","183");
        //joB=callerSalesArrangementService.call("GetSalesArrangement", joA);
        //log.info(joB.toString(2));

        joA = new JSONObject();
        joA.put("SalesArrangementId","186");
        joB=callerSalesArrangementService.call("GetSalesArrangement", joA);
        log.info(joB.toString(2));        
    }

    void test_list_sa()
    {

        JSONObject joA = new JSONObject();
        JSONObject joB = new JSONObject();
        JSONObject joC = new JSONObject();
        
        joA.put("CaseId","2954822");
        joB=callerSalesArrangementService.call("GetSalesArrangementList", joA);
        log.info(joB.toString(2));
    }

    void test_SearchCustomers() throws InterruptedException
    {

        JSONObject joA = new JSONObject();
        JSONObject joB = new JSONObject();
        JSONObject joC = new JSONObject();


        //grpcurl -insecure -d "{\"Identity\":{\"identityId\":928532258,\"identityScheme\":2}}" -H "Authorization: Basic YTph" 172.30.35.51:31004 DomainServices.CustomerService.V1.CustomerService/SearchCustomers
        joA.put("Identity",(new JSONObject()).put("identityId",928532258).put("identityScheme",2));
        joA.put("Mandant",2);
        log.info(joA.toString(2));
        joB=callerCustomerService.call("SearchCustomers",joA);  
        log.info(joB.toString(2));


    }

    void getUser() throws InterruptedException
    {

        JSONObject joA = new JSONObject();
        JSONObject joB = new JSONObject();
        JSONObject joC = new JSONObject();

        joA = new JSONObject();
        joB = new JSONObject();
        joC = new JSONObject();
/*        
        joA.put("UserId",948);
        log.info(joA.toString(2));
        joB=callerUserService.call("GetUser",joA);  
        log.info(joB.toString(2));
*/
        joA.put("Login","99806569");
        log.info(joA.toString(2));
        joB=callerUserService.call("GetUserByLogin",joA);  
        log.info(joB.toString(2));
    }



    void searchCustomer() throws InterruptedException
    {
/*
        JSONObject joA = new JSONObject();
        JSONObject joB = new JSONObject();
        JSONObject joC = new JSONObject();
        joC.put("identityId",926961964);
        joC.put("identityScheme","Mp");
        joA.put("Identity",joC);
        joB=callerCustomerService.call("GetCustomerDetail",joA);

*/

        JSONObject IsearchCustomer = new JSONObject();
        JSONObject OsearchCustomer = new JSONObject();



        IsearchCustomer.put("NaturalPerson",(new JSONObject())
                .put("LastName","test_mikel")
        )
        .put("Mandant",1);


        //IsearchCustomer.put("PhoneNumber","775583453");
        OsearchCustomer = callerCustomerService.call("SearchCustomers",IsearchCustomer);
        log.info(OsearchCustomer.toString(2));
        log.info(""+OsearchCustomer.getJSONArray("Customers").length());
        JSONObject customer=null;
        for(int i=0;i<(OsearchCustomer.getJSONArray("Customers").length());i++)
        {
            customer=OsearchCustomer.getJSONArray("Customers").getJSONObject(i);

            if(customer.getJSONArray("Identities").length()==2)
                break;

            customer=null;

            /*if(customer.getString("Street").equals("Zelená 736/6"))
                break;
            if(customer.getJSONArray("Identities").getJSONObject(0).getNumber("identityId").equals(970068946))
                break;*/
            //TODO: korektne osetrit, ze ho nenajdu
        }

        if(customer!=null)
            log.info(customer.toString(2));    

        /*JSONObject InGetCustomer=(new JSONObject()).put("Identity",customer.getJSONArray("Identities").getJSONObject(0));
        JSONObject OutGetCustomer = callerCustomerService.call("GetCustomerDetail",InGetCustomer);
        log.info(OutGetCustomer.toString(2));*/
        /*
    "Identities": [{
      "identityId": 928533189,
      "identityScheme": "Kb"
    }],        
        */

    }


    void test_identitySchemesCodebook() throws InterruptedException
    {
        JSONObject joA = new JSONObject();
        JSONObject joB = new JSONObject();
        JSONObject joC = new JSONObject();
        //joA.put("","");
        joB=callerCodebookService.call("IdentitySchemes", joA);
        //joB=callerCodebookService.call("SalesArrangementTypes", joA);
        log.info(joB.toString(2));
    }

    void test_deleteCase() throws InterruptedException
    {
/*
        HashMap<String, String> additionalHeaders=new HashMap<String, String>();
        additionalHeaders.put("Correlation-Context","MpPartyId=3048");
        additionalHeaders.put("traceparent","00-ddc1760e36a462c9c03b2583b1c9a098-ea157dc423037e71-01");

        int ContractNumber=1234567891;

        JSONObject IsimulateMortgage = new JSONObject();

        IsimulateMortgage.put("ResourceProcessId","4D115798-0E05-4CF0-8A5A-1A3F871B3727")
                .put("SimulationInputs",(new JSONObject())
                        .put("ProductTypeId",20001)
                        .put("LoanAmount",toGrpcDecimal(1000000))
                        .put("CollateralAmount",toGrpcDecimal(2000000))
                        .put("LoanKindId",2000)
                        .put("LoanDuration",300)
                        .put("FixedRatePeriod",60)
                        .put("LoanPurposes",(new JSONArray()).put((new JSONObject())
                                .put("LoanPurposeId",202)
                                .put("Sum",toGrpcDecimal(1000000)))
                        )
                        //.put("SimulationToggleSettings",1)
                        //.put("StatementTypeId",1)
                        .put("MarketingActions",(new JSONObject())
                        )
                        .put("GuaranteeDateFrom",(new JSONObject())
                            .put("year",2022)
                            .put("month",7)
                            .put("day",15)
                        )
                );

        JSONObject OsimulateMortgage=callerOfferService.call("SimulateMortgage",IsimulateMortgage);
        log.info(OsimulateMortgage.toString(2));



        //if(true)
        //    return;


        JSONObject IsearchCustomer = new JSONObject();
        JSONObject OsearchCustomer = new JSONObject();

        IsearchCustomer.put("NaturalPerson",(new JSONObject())
                .put("LastName","novotný")
                
        ).put("Mandant",2);

        OsearchCustomer = callerCustomerService.call("SearchCustomers",IsearchCustomer);
        log.info(OsearchCustomer.toString(2));

        JSONObject customer;
        for(int i=0;;i++)
        {
            customer=OsearchCustomer.getJSONArray("Customers").getJSONObject(i);

            if(customer.getString("Street").equals("Zelená 736/6"))
                break;
            if(customer.getJSONArray("Identities").getJSONObject(0).getNumber("identityId").equals(970068946))
                break;
            //TODO: korektne osetrit, ze ho nenajdu
        }

        log.info(customer.toString(2));

        JSONObject IcreateCase = new JSONObject();
        JSONObject OcreateCase = new JSONObject();

        IcreateCase
                .put("CaseOwnerUserId",3048)
                //.put("CaseOwnerUserId",267)
                .put("Customer",(new JSONObject())
                    .put("Name",customer.getJSONObject("NaturalPerson").getString("LastName"))
                    .put("FirstNameNaturalPerson",customer.getJSONObject("NaturalPerson").getString("FirstName"))
                    .put("Identity",customer.getJSONArray("Identities").getJSONObject(0))
                    .put("Cin","")
                    .put("DateOfBirthNaturalPerson",customer.getJSONObject("NaturalPerson").getJSONObject("DateOfBirth"))
                ).put("Data",(new JSONObject())
                    .put("ProductTypeId",20001)
                    .put("ContractNumber",ContractNumber)
                    .put("TargetAmount",(new JSONObject())
                        .put("units",1000000)
                        .put("nanos",0)
                )
        );


        log.info("IcreateCase:");
        log.info(IcreateCase.toString(2));
        OcreateCase = callerCaseService.call("CreateCase",IcreateCase,additionalHeaders);
        log.info("OcreateCase:");
        log.info(OcreateCase.toString(2));
        //OcreateCase=new JSONObject("{\"CaseId\": \"85\"}");
        String CaseId=OcreateCase.getString("CaseId");

        log.info("CaseId:");
        log.info(CaseId);



        JSONObject IcreateSalesArrangement=new JSONObject();
        JSONObject OcreateSalesArrangement;

        IcreateSalesArrangement.put("CaseId",CaseId)
            .put("SalesArrangementTypeId",1)
            .put("ContractNumber",ContractNumber)
            .put("OfferId",OsimulateMortgage.getNumber("OfferId"));


        OcreateSalesArrangement=callerSalesArrangementService.call("CreateSalesArrangement",IcreateSalesArrangement);
        log.info(OcreateSalesArrangement.toString(2));

*/

        JSONObject In = new JSONObject();
        JSONObject Out = new JSONObject(); 
        In.put("CaseId",2954688);  // <----------------------------
       
        Out=callerCaseService.call("DeleteCase",In);
        log.info(Out.toString(2));


    }

    void test_updateCaseState() throws InterruptedException
    {
        JSONObject In = new JSONObject();
        JSONObject Out = new JSONObject();
        In.put("CaseId",2954643);
        In.put("State",4);   // <----------------------------
        
        Out=callerCaseService.call("UpdateCaseState",In);
        log.info(Out.toString(2));
        
    }


    void test_caseTasks() throws InterruptedException
    {
        

        HashMap<String, String> gatTaskListHeaders=new HashMap<String, String>();
        gatTaskListHeaders.put("Correlation-Context","MpPartyId=3048");
        gatTaskListHeaders.put("traceparent","00-ddc1760e36a462c9c03b2583b1c9a098-ea157dc423037e71-01");

/*
        JSONObject SearchCasesIn = new JSONObject();
        JSONObject SearchCasesOut = new JSONObject();
        SearchCasesIn.put("CaseOwnerUserId",3048);
        SearchCasesOut=callerCaseService.call("SearchCases",SearchCasesIn,gatTaskListHeaders);
        log.info(SearchCasesOut.toString(2));
*/


        JSONObject GetTaskListIn = new JSONObject();
        JSONObject GetTaskListOut = new JSONObject();
        GetTaskListIn.put("CaseId",2954490);
        //GetTaskListIn.put("CaseId",2954502);
        //GetTaskListOut=callerCaseServiceOtherUser.call("GetTaskList",GetTaskListIn);
        GetTaskListOut=callerCaseService.call("GetTaskList",GetTaskListIn,gatTaskListHeaders);
        log.info(GetTaskListOut.toString(2));


    }

    void test_get_case()
    {

        JSONObject joA = new JSONObject();
        JSONObject joB = new JSONObject();
        JSONObject joC = new JSONObject();
        
        joA.put("CaseId","2954822");
        joB=callerCaseService.call("GetCaseDetail", joA);
        log.info(joB.toString(2));
/*
        try {
            PrintWriter out = new PrintWriter("out.txt");
            out.println(joB.toString(2));
            out.close();
        }
        catch (Exception e)
        {

        }
*/
    }


    void test_cm_instance() throws InterruptedException
    {
        JSONObject IsearchCustomer = new JSONObject();
        JSONObject OsearchCustomer = new JSONObject();

        IsearchCustomer.put("NaturalPerson",(new JSONObject())
                .put("LastName","novotný")
                .put("FirstName","jan")
        );

        OsearchCustomer = callerCustomerService.call("SearchCustomers",IsearchCustomer);
        log.info(OsearchCustomer.toString(2));

        //144958744
        //702813620
    }


    void test_e2e_json() throws InterruptedException
    {

        int ContractNumber=1234567891;

        JSONObject IsimulateMortgage = new JSONObject();

        IsimulateMortgage.put("ResourceProcessId","4D115798-0E05-4CF0-8A5A-1A3F871B3727")
                .put("Inputs",(new JSONObject())
                        .put("ProductTypeId",20001)
                        .put("LoanAmount",toGrpcDecimal(1000000))
                        .put("CollateralAmount",toGrpcDecimal(2000000))
                        .put("LoanKindId",2000)
                        .put("LoanDuration",300)
                        .put("FixedRatePeriod",60)
                        .put("LoanPurpose",(new JSONArray()).put((new JSONObject())
                                .put("LoanPurposeId",202)
                                .put("Sum",toGrpcDecimal(1000000)))
                        ).put("SimulationToggleSettings",1)
                        .put("StatementTypeId",1)
                );

        JSONObject OsimulateMortgage=callerOfferService.call("SimulateMortgage",IsimulateMortgage);
        //log.info(OsimulateMortgage.toString(2));



        //if(true)
        //    return;


        JSONObject IsearchCustomer = new JSONObject();
        JSONObject OsearchCustomer = new JSONObject();

        IsearchCustomer.put("NaturalPerson",(new JSONObject())
                .put("LastName","novotnÃ½")
        );

        OsearchCustomer = callerCustomerService.call("SearchCustomers",IsearchCustomer);
        log.info(OsearchCustomer.toString(2));

        JSONObject customer;
        for(int i=0;;i++)
        {
            customer=OsearchCustomer.getJSONArray("Customers").getJSONObject(i);

            if(customer.getString("Street").equals("ZelenÃ¡ 736/6"))
                break;
            if(customer.getJSONArray("Identities").getJSONObject(0).getNumber("identityId").equals(970068946))
                break;
            //TODO: korektne osetrit, ze ho nenajdu
        }

        log.info(customer.toString(2));

        JSONObject IcreateCase = new JSONObject();
        JSONObject OcreateCase = new JSONObject();

        IcreateCase
                .put("CaseOwnerUserId",3048)
                .put("Customer",(new JSONObject())
                        .put("Name",customer.getJSONObject("NaturalPerson").getString("LastName"))
                        .put("FirstNameNaturalPerson",customer.getJSONObject("NaturalPerson").getString("FirstName"))
                        .put("Identity",customer.getJSONArray("Identities").getJSONObject(0)
                        ).put("Cin","")
                        .put("DateOfBirthNaturalPerson",customer.getJSONObject("NaturalPerson").getJSONObject("DateOfBirth"))
                ).put("Data",(new JSONObject())
                .put("ProductTypeId",20001)
                .put("ContractNumber",ContractNumber)
                .put("TargetAmount",(new JSONObject())
                        .put("units",1000000)
                        .put("nanos",0)
                )
        );



        log.info(IcreateCase.toString(2));
        OcreateCase = callerCaseService.call("CreateCase",IcreateCase);
        //OcreateCase=new JSONObject("{\"CaseId\": \"85\"}");

        JSONObject IcreateSalesArrangement=new JSONObject();
        JSONObject OcreateSalesArrangement;

        IcreateSalesArrangement.put("CaseId",OcreateCase.getString("CaseId"))
            .put("SalesArrangementTypeId",1)
            .put("ContractNumber",ContractNumber)
            .put("OfferId",OsimulateMortgage.getNumber("OfferId"));


        OcreateSalesArrangement=callerSalesArrangementService.call("CreateSalesArrangement",IcreateSalesArrangement);
        log.info(OcreateSalesArrangement.toString(2));

        JSONObject IcreateCustomerOnSA1=new JSONObject();
        IcreateCustomerOnSA1.put("SalesArrangementId",OcreateSalesArrangement.getNumber("SalesArrangementId"))
                .put("CustomerRoleId",1)
                .put("Customer",(new JSONObject())
                        .put("CustomerIdentifiers",customer.getJSONArray("Identities"))
                );
        JSONObject OcreateCustomerOnSA1=callerCustomerOnSAService.call("CreateCustomer",IcreateCustomerOnSA1);


        //OcreateSalesArrangement=new JSONObject("{\"SalesArrangementId\": 22}");

        JSONObject IcreateHosehold=new JSONObject(OcreateSalesArrangement.toString());
        IcreateHosehold.put("HouseholdTypeId",1);
        IcreateHosehold.put("CustomerOnSAId1",OcreateCustomerOnSA1.getNumber("CustomerOnSAId"));

        JSONObject OcreateHosehold=callerHouseholdService.call("CreateHousehold",IcreateHosehold);
        log.info(OcreateHosehold.toString(2));

        JSONObject OsendToCmp=callerSalesArrangementService.call("SendToCmp",OcreateSalesArrangement);
        log.info(OsendToCmp.toString(2));




    }


    void test_adpra192() throws InterruptedException
    {
        log.info("Started test_adpra192()");

        JSONObject joA = new JSONObject();
        JSONObject joB = new JSONObject();
        JSONObject joC = new JSONObject();


        joA = new JSONObject();
        joB = new JSONObject();
        joC = new JSONObject();
        joA.put("Environment","FAT");
        joA.put("ServiceType","Grpc");
        log.info(joA.toString(2));
        joB=callerServiceDiscovery.call("GetServices", joA);
        log.info(joB.toString(2));

        joA = new JSONObject();
        joB = new JSONObject();
        joC = new JSONObject();
        joA.put("OfferId","26");
        joB=callerOfferService.call("GetOffer",joA);
        log.info(joB.toString(2));

        joA = new JSONObject();
        joB = new JSONObject();
        joC = new JSONObject();
        joC.put("identityId",926961964);
        joC.put("identityScheme","Mp");
        joA.put("Identity",joC);
        joB=callerCustomerService.call("GetCustomerDetail",joA);
        //log.info(joB.toString(2));

        joA = new JSONObject();
        joB = new JSONObject();
        joC = new JSONObject();
        joA.put("UserId",3048);
        joB=callerUserService.call("GetUser",joA);
        //log.info(joB.toString(2));

        joA = new JSONObject();
        joB = new JSONObject();
        joC = new JSONObject();
        joA.put("CaseId","1");
        joB=callerCaseService.call("GetCaseDetail", joA);
        //log.info(joB.toString(2));


        joA = new JSONObject();
        joB = new JSONObject();
        joC = new JSONObject();
        joA.put("SalesArrangementId","1");
        joB=callerSalesArrangementService.call("GetSalesArrangement", joA);
        //log.info(joB.toString(2));

        joA = new JSONObject();
        joB = new JSONObject();
        joC = new JSONObject();
        joA.put("CaseId","1");
        joB=callerProductService.call("GetProductList", joA);
        //log.info(joB.toString(2));


        joA = new JSONObject();
        joB = new JSONObject();
        joC = new JSONObject();
        //joA.put("","");
        joB=callerCodebookService.call("MyTestCodebook", joA);
        //log.info(joB.toString(2));



        /*
        // Storage si asi nepovida
        joA = new JSONObject();
        joB = new JSONObject();
        joA.put("ApplicationKey","JakubVana-0001");
        joA.put("BlobData","Moje skvela data.");
        joA.put("SessionId","0001");
        joB=callerStorage.call("Save",joA);
        log.info(joB.toString(2));
        */


        return;
/*
        message GetServicesRequest {
        string Environment = 1;
        ServiceTypes ServiceType = 2;
    }
        message GetServicesResponse {
        repeated DiscoverableService Services = 1;
        string EnvironmentName = 2;
    }

        enum ServiceTypes {
            Unknown = 0;
            Grpc = 1;
            Rest = 2;
            Proprietary = 3;
        }
*/
/*
        joA.put("CaseId","1");
        joB=callerCaseService.call("GetCaseDetail", joA);
        //log.info(joB.toString(2));

        joC=joB.getJSONObject("Data").getJSONObject("TargetAmount");
        joC.put("units",""+(joC.getNumber("units").intValue()+1));
        //log.info(joC.toString(2));
        //log.info(joB.toString(2));

        joA.put("Data",joB.getJSONObject("Data"));
        joB=callerCaseService.call("UpdateCaseData", joA);

        joA = new JSONObject();
        joA.put("CaseId","1");
        joB=callerCaseService.call("GetCaseDetail", joA);
        log.info(joB.toString(2));

        //Thread.sleep(10000);

        joA = new JSONObject();
        joB = new JSONObject();
        joC = new JSONObject();

        joA.put("OfferId","25");

        joB=callerOfferService.call("GetOffer",joA);
        log.info(joB.toString(2));

        joB=callerOfferService.call("GetMortgageData",joA);
        log.info(joB.toString(2));
*/

    }

}
