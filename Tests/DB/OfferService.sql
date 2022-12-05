---------------------------
-- CLEAR DB
---------------------------
PRAGMA writable_schema = 1;
DELETE FROM sqlite_master;
PRAGMA writable_schema = 0;
VACUUM;
PRAGMA integrity_check;
---------------------------

-- SET foreign keys To OFF
PRAGMA foreign_keys = OFF;

-- CREATE TABLES
CREATE TABLE SimulateMortgageRequest (SimulateMortgageRequestPk INTEGER PRIMARY KEY, ResourceProcessId TEXT NOT NULL /*string*/, BasicParametersPk INTEGER NULL /*None*/, SimulationInputsPk INTEGER NULL /*None*/);
CREATE TABLE BasicParameters (BasicParametersPk INTEGER PRIMARY KEY, FinancialResourcesOwn REAL NULL /*NullableGrpcDecimal*/, FinancialResourcesOther REAL NULL /*NullableGrpcDecimal*/, GuaranteeDateTo REAL NULL /*NullableGrpcDate*/, StatementTypeId INTEGER NULL /*Int32Value*/);
CREATE TABLE MortgageSimulationInputs (MortgageSimulationInputsPk INTEGER PRIMARY KEY, ExpectedDateOfDrawing REAL NULL /*NullableGrpcDate*/, ProductTypeId INTEGER NOT NULL /*int32*/, LoanKindId INTEGER NOT NULL /*int32*/, LoanAmount REAL NOT NULL 
/*GrpcDecimal*/, LoanDuration INTEGER NULL /*Int32Value*/, InterestRateDiscount REAL NULL /*NullableGrpcDecimal*/, FixedRatePeriod INTEGER NULL /*Int32Value*/, DrawingTypeId INTEGER NULL /*Int32Value*/, DrawingDurationId INTEGER NULL /*Int32Value*/, PaymentDay INTEGER NULL /*Int32Value*/, IsEmployeeBonusRequested INTEGER NULL /*BoolValue*/, DeveloperPk INTEGER NULL /*None*/, FeeSettingsPk INTEGER NULL /*None*/, MarketingActionsPk INTEGER NULL /*None*/, RiskLifeInsurancePk INTEGER NULL /*None*/, RealEstateInsurancePk INTEGER NULL /*None*/);
CREATE TABLE Developer (DeveloperPk INTEGER PRIMARY KEY, DeveloperId INTEGER NULL /*Int32Value*/, ProjectId INTEGER NULL /*Int32Value*/, NewDeveloperName TEXT NOT NULL /*string*/, NewDeveloperProjectName TEXT NOT NULL /*string*/, NewDeveloperCin TEXT NOT NULL /*string*/);
CREATE TABLE LoanPurpose (LoanPurposePk INTEGER PRIMARY KEY, MortgageSimulationInputsPk INTEGER NOT NULL, LoanPurposeId INTEGER NOT NULL /*int32*/, Sum REAL NOT NULL /*GrpcDecimal*/, FOREIGN KEY(LoanPurposePk) REFERENCES MortgageSimulationInputs(MortgageSimulationInputsPk));
CREATE TABLE FeeSettings (FeeSettingsPk INTEGER PRIMARY KEY, FeeTariffPurpose INTEGER NULL /*Int32Value*/, IsStatementCharged INTEGER NOT NULL /*bool*/);
CREATE TABLE InputMarketingAction (InputMarketingActionPk INTEGER PRIMARY KEY, Domicile INTEGER NOT NULL /*bool*/, HealthRiskInsurance INTEGER NOT NULL /*bool*/, RealEstateInsurance INTEGER NOT NULL /*bool*/, IncomeLoanRatioDiscount INTEGER NOT NULL /*bool*/, UserVip INTEGER NOT NULL /*bool*/);
CREATE TABLE InputFee (InputFeePk INTEGER PRIMARY KEY, MortgageSimulationInputsPk INTEGER NOT NULL, FeeId INTEGER NOT NULL /*int32*/, DiscountPercentage REAL NOT NULL /*GrpcDecimal*/, FOREIGN KEY(InputFeePk) REFERENCES MortgageSimulationInputs(MortgageSimulationInputsPk));
CREATE TABLE RiskLifeInsurance (RiskLifeInsurancePk INTEGER PRIMARY KEY, Sum REAL NOT NULL /*GrpcDecimal*/, Frequency INTEGER NULL /*Int32Value*/);
CREATE TABLE RealEstateInsurance (RealEstateInsurancePk INTEGER PRIMARY KEY, Sum REAL NOT NULL /*GrpcDecimal*/, Frequency INTEGER NULL /*Int32Value*/);


-- SET foreign keys To ON
PRAGMA foreign_keys = ON;
--PRAGMA foreign_keys;
