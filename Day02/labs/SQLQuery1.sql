CREATE DATABASE CareBridgeDB;   
USE CareBridgeDB;


CREATE TABLE Department (
    DepartmentId   INT IDENTITY(1,1) PRIMARY KEY,
    Name           NVARCHAR(100) NOT NULL,
    Location       NVARCHAR(100) NULL
);

CREATE TABLE Provider (
    ProviderId     INT IDENTITY(1,1) PRIMARY KEY,
    FullName       NVARCHAR(150) NOT NULL,
    Specialty      NVARCHAR(100) NOT NULL,
    DepartmentId   INT NOT NULL
        REFERENCES Department(DepartmentId)
);


CREATE TABLE Patient (
    PatientId      INT IDENTITY(1,1) PRIMARY KEY,
    MRN            NVARCHAR(20) NOT NULL UNIQUE,
    FullName       NVARCHAR(150) NOT NULL,
    DateOfBirth    DATE NOT NULL,
    Gender         CHAR(1) NOT NULL
        CHECK (Gender IN ('M','F','O')),
    City           NVARCHAR(100) NULL,
    IsActive       BIT NOT NULL DEFAULT 1
);

CREATE TABLE Insurance (
    InsuranceId    INT IDENTITY(1,1) PRIMARY KEY,
    PatientId      INT NOT NULL REFERENCES Patient(PatientId),
    Payer          NVARCHAR(120) NOT NULL,
    PolicyNumber   NVARCHAR(50) NOT NULL,
    EffectiveDate  DATE NOT NULL,
    ExpiryDate     DATE NULL
);


CREATE TABLE Encounter (
    EncounterId    INT IDENTITY(1,1) PRIMARY KEY,
    PatientId      INT NOT NULL REFERENCES Patient(PatientId),
    ProviderId     INT NOT NULL REFERENCES Provider(ProviderId),
    DepartmentId   INT NOT NULL REFERENCES Department(DepartmentId),
    AdmitDate      DATETIME2 NOT NULL,
    DischargeDate  DATETIME2 NULL,
    EncounterType  NVARCHAR(30) NOT NULL
        CHECK (EncounterType IN ('Inpatient','Outpatient','ED'))
);

CREATE TABLE Diagnosis (
    DiagnosisId    INT IDENTITY(1,1) PRIMARY KEY,
    EncounterId    INT NOT NULL REFERENCES Encounter(EncounterId),
    IcdCode        NVARCHAR(10) NOT NULL,
    Description    NVARCHAR(200) NOT NULL,
    DiagnosedOn    DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE [Procedure] (
    ProcedureId    INT IDENTITY(1,1) PRIMARY KEY,
    EncounterId    INT NOT NULL REFERENCES Encounter(EncounterId),
    CptCode        NVARCHAR(10) NOT NULL,
    Description    NVARCHAR(200) NOT NULL,
    PerformedOn    DATETIME2 NOT NULL
);

CREATE TABLE Claim (
    ClaimId        INT IDENTITY(1,1) PRIMARY KEY,
    EncounterId    INT NOT NULL REFERENCES Encounter(EncounterId),
    InsuranceId    INT NOT NULL REFERENCES Insurance(InsuranceId),
    BilledAmount   DECIMAL(12,2) NOT NULL,
    ReimbursedAmt  DECIMAL(12,2) NULL,
    Status         NVARCHAR(20) NOT NULL
        CHECK (Status IN ('Submitted','Paid','Denied'))
);

ALTER TABLE Encounter
ALTER COLUMN ProviderId INT NULL;

SELECT
    d.Name AS Department,
    COUNT(*) AS FluEncounters
FROM Encounter e
JOIN Diagnosis dx
    ON dx.EncounterId = e.EncounterId
JOIN Department d
    ON d.DepartmentId = e.DepartmentId
WHERE dx.IcdCode LIKE 'J1[01]%'
  AND e.AdmitDate >= CAST(GETDATE() AS DATE)
GROUP BY d.Name
ORDER BY FluEncounters DESC;



ALTER TABLE Patient
ADD
    ValidFrom DATETIME2
        GENERATED ALWAYS AS ROW START HIDDEN
        CONSTRAINT DF_Patient_From
        DEFAULT SYSUTCDATETIME(),

    ValidTo DATETIME2
        GENERATED ALWAYS AS ROW END HIDDEN
        CONSTRAINT DF_Patient_To
        DEFAULT '9999-12-31 23:59:59.9999999',

    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo);


ALTER TABLE Patient
SET (
    SYSTEM_VERSIONING = ON
    (
        HISTORY_TABLE = dbo.Patient_History
    )
);


select * from [dbo].[Patient] where PatientId=1;


update Patient
Set city='Hyd'
where PatientId =1;

SELECT
    PatientId,
    City,
    ValidFrom,
    ValidTo
FROM Patient
FOR SYSTEM_TIME ALL
WHERE PatientId = 1
ORDER BY ValidFrom;
