﻿using DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Abstraction;
using Grpc.Core;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.CisTypes;

namespace DomainServices.OfferService.Api.Handlers;

internal class SimulateMortgageHandler
    : BaseHandler, IRequestHandler<Dto.SimulateMortgageMediatrRequest, SimulateMortgageResponse>
{
    #region Construction

    private readonly ILogger<SimulateMortgageHandler> _logger;
    private readonly Eas.IEasClient _easClient;

    public SimulateMortgageHandler(
        Repositories.OfferRepository repository,
        ILogger<SimulateMortgageHandler> logger,
        ICodebookServiceAbstraction codebookService,
        Eas.IEasClient easClient
        ) : base(repository, codebookService)
    {
        _logger = logger;
        _easClient = easClient;
    }

    #endregion

    public async Task<SimulateMortgageResponse> Handle(Dto.SimulateMortgageMediatrRequest request, CancellationToken cancellation)
    {
        // kontrola ProductTypeId (zda je typu Mortgage)
        await CheckProductTypeCategory(
            request.Request.SimulationInputs.ProductTypeId,
            CodebookService.Contracts.Endpoints.ProductTypes.ProductTypeCategory.Mortgage
        );

        var resourceProcessId = Guid.Parse(request.Request.ResourceProcessId);

        var basicParameters = request.Request.BasicParameters;

        // setup input default values
        // var inputs = SetUpInputDefaults(request.Request.SimulationInputs);
        var inputs = request.Request.SimulationInputs;

        // get simulation outputs
        //var result = getEasResult(await _easClient.RunSimulation(inputs));
        var results = this.GenerateFakeSimulationResults(inputs);

        var easInputs = ToEasInput(inputs);
        var easSimulation = ResolveRunSimulation(await _easClient.RunSimulation(easInputs));

        // save to DB
        var entity = await _repository.SaveOffer(resourceProcessId, basicParameters, inputs, results, cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.Offer), entity.OfferId);

        // create response
        return new SimulateMortgageResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new ModificationStamp(entity),
            BasicParameters = basicParameters,
            SimulationInputs = inputs,
            SimulationResults = results,
        };

    }

    private SimulationInputs SetUpInputDefaults(SimulationInputs inputs)
    {
        inputs.ExpectedDateOfDrawing = inputs.ExpectedDateOfDrawing ?? DateTime.Now.AddDays(1); //currentDate + 1D
        inputs.PaymentDay = inputs.PaymentDay ?? 15;

        return inputs;
    }

    // TODO: redirect to EAS
    private SimulationResults GenerateFakeSimulationResults(SimulationInputs input)
    {

        int loanDuration = (input.LoanDuration ?? 0);

        var results = new SimulationResults
        {
            //ProductTypeId = input.ProductTypeId,
            LoanInterestRate = 0.02m,                       //mock: 0.02
            //InterestRateAnnounced = 1,                      //mock: 1 (povinné v DV)
            LoanAmount = input.LoanAmount,                  //mock: ze vstupu
            LoanDuration = loanDuration,                    //mock: 0 (pokud na vstupu nezadáno)
                                                            //mock: (náhodné číslo generované např. jako výše úvěru / splatností)
            LoanPaymentAmount = loanDuration == 0 ? 0 : input.LoanAmount / loanDuration,
            LoanToValue = input.LoanToValue,                //mock: ze vstupu
            //LoanToCost = 0.0m,                               //mock: (celková výše investice / celková výše vlastních zdrojů) ... neznáme vlastní zdroje
            Aprc = 0.25m,                                    //mock: 0.25
            LoanTotalAmount = (input.LoanAmount + 1000000), //mock: (vstupní hodnota výše úvěru + 1 000 000)
                                                            //StatementTypeId = 1,                            //mock: 1
                                                            //mock: currentDate + 1D je defaultní hodnota při nezadání
                                                            //ExpectedDateOfDrawing = (input.ExpectedDateOfDrawing == null) ? new GrpcDate(DateTime.Now.AddDays(1)) : new GrpcDate(input.ExpectedDateOfDrawing.Year, input.ExpectedDateOfDrawing.Month, input.ExpectedDateOfDrawing.Day),
                                                            //mock: 15 je defaultní hodnota při nezadání
                                                            //PaymentDayOfTheMonth = input.PaymentDayOfTheMonth ?? 15,
        };

        //output.LoanPurpose.AddRange(input.LoanPurpose);     //mock: ze vstupu

        return results;
    }

    private ExternalServices.Eas.R21.EasWrapper.ESBI_SIMULATION_INPUT_PARAMETERS ToEasInput(SimulationInputs inputs)
    {
        return new ExternalServices.Eas.R21.EasWrapper.ESBI_SIMULATION_INPUT_PARAMETERS
        {
            
            
        };
    }

    private static object ResolveRunSimulation(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<ExternalServices.Eas.Dto.CreateNewOrGetExisingClientResponse> r => r.Model,
            ErrorServiceCallResult r => default(int?), //TODO co se ma v tomhle pripade delat?
            _ => throw new NotImplementedException("RunSimulation")
        };




    //static int? resolveCreateEasClient(IServiceCallResult result) =>
    //    result switch
    //    {
    //        SuccessfulServiceCallResult<ExternalServices.Eas.Dto.CreateNewOrGetExisingClientResponse> r => r.Model.Id,
    //        ErrorServiceCallResult r => default(int?), //TODO co se ma v tomhle pripade delat?
    //        _ => throw new NotImplementedException("resolveCreateEasClient")
    //    };


    //private void ResolveAddFirstSignatureDate(IServiceCallResult result)
    //{
    //    switch (result)
    //    {
    //        case ErrorServiceCallResult err:
    //            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors[0].Message, err.Errors[0].Key);
    //    }
    //}

    //private void CheckMpHomeResult(IServiceCallResult result)
    //{
    //    switch (result)
    //    {
    //        case ErrorServiceCallResult err:
    //            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors.First().Message, err.Errors.First().Key);
    //    }
    //}


}

