-- CREATE TABLE SimulateMortgageRequest (SimulateMortgageRequestPk INTEGER PRIMARY KEY, ResourceProcessId TEXT NOT NULL /*string*/, BasicParametersPk INTEGER NULL /*None*/, SimulationInputsPk INTEGER NULL /*None*/);
-- CREATE TABLE BasicParameters (BasicParametersPk INTEGER PRIMARY KEY, FinancialResourcesOwn REAL NULL /*NullableGrpcDecimal*/, FinancialResourcesOther REAL NULL /*NullableGrpcDecimal*/, GuaranteeDateTo REAL NULL /*NullableGrpcDate*/, StatementTypeId INTEGER NULL /*Int32Value*/);
-- CREATE TABLE MortgageSimulationInputs (MortgageSimulationInputsPk INTEGER PRIMARY KEY, ExpectedDateOfDrawing REAL NULL /*NullableGrpcDate*/, ProductTypeId INTEGER NOT NULL /*int32*/, LoanKindId INTEGER NOT NULL /*int32*/, LoanAmount REAL NOT NULL 
-- /*GrpcDecimal*/, LoanDuration INTEGER NULL /*Int32Value*/, InterestRateDiscount REAL NULL /*NullableGrpcDecimal*/, FixedRatePeriod INTEGER NULL /*Int32Value*/, DrawingTypeId INTEGER NULL /*Int32Value*/, DrawingDurationId INTEGER NULL /*Int32Value*/, PaymentDay INTEGER NULL /*Int32Value*/, IsEmployeeBonusRequested INTEGER NULL /*BoolValue*/, DeveloperPk INTEGER NULL /*None*/, FeeSettingsPk INTEGER NULL /*None*/, MarketingActionsPk INTEGER NULL /*None*/, RiskLifeInsurancePk INTEGER NULL /*None*/, RealEstateInsurancePk INTEGER NULL /*None*/);
-- CREATE TABLE Developer (DeveloperPk INTEGER PRIMARY KEY, DeveloperId INTEGER NULL /*Int32Value*/, ProjectId INTEGER NULL /*Int32Value*/, NewDeveloperName TEXT NOT NULL /*string*/, NewDeveloperProjectName TEXT NOT NULL /*string*/, NewDeveloperCin TEXT NOT NULL /*string*/);
-- CREATE TABLE LoanPurpose (LoanPurposePk INTEGER PRIMARY KEY, MortgageSimulationInputsPk INTEGER NOT NULL, LoanPurposeId INTEGER NOT NULL /*int32*/, Sum REAL NOT NULL /*GrpcDecimal*/, FOREIGN KEY(LoanPurposePk) REFERENCES MortgageSimulationInputs(MortgageSimulationInputsPk));
-- CREATE TABLE FeeSettings (FeeSettingsPk INTEGER PRIMARY KEY, FeeTariffPurpose INTEGER NULL /*Int32Value*/, IsStatementCharged INTEGER NOT NULL /*bool*/);
-- CREATE TABLE InputMarketingAction (InputMarketingActionPk INTEGER PRIMARY KEY, Domicile INTEGER NOT NULL /*bool*/, HealthRiskInsurance INTEGER NOT NULL /*bool*/, RealEstateInsurance INTEGER NOT NULL /*bool*/, IncomeLoanRatioDiscount INTEGER NOT NULL /*bool*/, UserVip INTEGER NOT NULL /*bool*/);
-- CREATE TABLE InputFee (InputFeePk INTEGER PRIMARY KEY, MortgageSimulationInputsPk INTEGER NOT NULL, FeeId INTEGER NOT NULL /*int32*/, DiscountPercentage REAL NOT NULL /*GrpcDecimal*/, FOREIGN KEY(InputFeePk) REFERENCES MortgageSimulationInputs(MortgageSimulationInputsPk));
-- CREATE TABLE RiskLifeInsurance (RiskLifeInsurancePk INTEGER PRIMARY KEY, Sum REAL NOT NULL /*GrpcDecimal*/, Frequency INTEGER NULL /*Int32Value*/);
-- CREATE TABLE RealEstateInsurance (RealEstateInsurancePk INTEGER PRIMARY KEY, Sum REAL NOT NULL /*GrpcDecimal*/, Frequency INTEGER NULL /*Int32Value*/);


--------------------------------------------------------------------------------------------------------------------
-- InputMarketingAction | InputMarketingActionPks: [1, 2, 3]
--------------------------------------------------------------------------------------------------------------------
INSERT INTO InputMarketingAction(InputMarketingActionPk, Domicile, HealthRiskInsurance, RealEstateInsurance, IncomeLoanRatioDiscount, UserVip) VALUES 
(1, 1, 1, 1, 1, 1),
(2, 0, 0, 0, 0, 0),
(3, 1, 0, 1, 0, 1);


--------------------------------------------------------------------------------------------------------------------
-- MortgageSimulationInputs | MortgageSimulationInputsPks: [1, 2, 3]
--------------------------------------------------------------------------------------------------------------------
INSERT INTO MortgageSimulationInputs(MortgageSimulationInputsPk, ExpectedDateOfDrawing, ProductTypeId, LoanKindId, LoanAmount, LoanDuration, InterestRateDiscount, FixedRatePeriod, DrawingTypeId, DrawingDurationId, PaymentDay, IsEmployeeBonusRequested, DeveloperPk, FeeSettingsPk, MarketingActionsPk, RiskLifeInsurancePk, RealEstateInsurancePk) VALUES
(1, julianday('2023-10-01'), 20001, 2000, 3000000.00, 36, NULL, 24, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL),
(2, julianday('2023-11-01'), 20001, 2000, 4000000.00, 36, NULL, 24, NULL, NULL, NULL, NULL, NULL, NULL, 2, NULL, NULL),
(3, julianday('2023-12-01'), 20001, 2000, 5000000.00, 36, NULL, 24, NULL, NULL, NULL, NULL, NULL, NULL, 3, NULL, NULL);
-- missing columns in table: GuaranteeDateFrom, CollateralAmount


--------------------------------------------------------------------------------------------------------------------
-- LoanPurpose
--------------------------------------------------------------------------------------------------------------------
INSERT INTO LoanPurpose(LoanPurposePk, MortgageSimulationInputsPk, LoanPurposeId, Sum) VALUES 
-- MortgageSimulationInputsPk = 1
(1, 1, 201, 1000000.00),
(2, 1, 202, 2000000.00),

-- MortgageSimulationInputsPk = 2
(3, 2, 201, 1000000.00),
(4, 2, 202, 2000000.00),

-- MortgageSimulationInputsPk = 3
(5, 3, 201, 1000000.00),
(6, 3, 202, 2000000.00);
-- incorrect foreign key MortgageSimulationInputsPk (the same problem in table InputFee)


--------------------------------------------------------------------------------------------------------------------
-- BasicParameters | BasicParametersPks: [1]
--------------------------------------------------------------------------------------------------------------------
INSERT INTO BasicParameters(BasicParametersPk, FinancialResourcesOwn, FinancialResourcesOther, GuaranteeDateTo, StatementTypeId) VALUES
(1, NULL, NULL, NULL, NULL);


--------------------------------------------------------------------------------------------------------------------
-- SimulateMortgageRequest | SimulateMortgageRequestPks: [1, 2, 3]
--------------------------------------------------------------------------------------------------------------------
INSERT INTO SimulateMortgageRequest(SimulateMortgageRequestPk, ResourceProcessId, BasicParametersPk, SimulationInputsPk) VALUES
(1, '00000000-0000-0000-1111-000000000000', 1, 1),
(2, '00000000-0000-0000-2222-000000000000', 1, 2),
(3, '00000000-0000-0000-3333-000000000000', 1, 3);