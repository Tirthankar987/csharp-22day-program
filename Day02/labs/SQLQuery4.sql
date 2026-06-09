CREATE OR ALTER PROCEDURE usp_ReadmissionAnalytics
    @WithinDays INT = 30
AS
BEGIN

    SET NOCOUNT ON;

    -- Build a patient timeline

    WITH OrderedEncounters AS (

        SELECT

            PatientId,
            EncounterId,
            AdmitDate,

            LAG(DischargeDate)
                OVER (
                    PARTITION BY PatientId
                    ORDER BY AdmitDate
                ) AS PreviousDischarge

        FROM Encounter

        WHERE EncounterType = 'Inpatient'

    )

    -- Find readmissions

    SELECT

        PatientId,
        EncounterId,
        AdmitDate,

        DATEDIFF(
            DAY,
            PreviousDischarge,
            AdmitDate
        ) AS DaysSincePreviousVisit

    FROM OrderedEncounters

    WHERE PreviousDischarge IS NOT NULL

    AND DATEDIFF(
            DAY,
            PreviousDischarge,
            AdmitDate
        ) <= @WithinDays;

END;


EXEC usp_ReadmissionAnalytics
    @WithinDays = 30;


CREATE OR ALTER VIEW vw_Clinical
AS

SELECT

    pt.MRN,
    pt.FullName,

    e.EncounterId,
    e.EncounterType,

    dx.IcdCode,
    dx.Description

FROM Patient pt

JOIN Encounter e
    ON e.PatientId = pt.PatientId

LEFT JOIN Diagnosis dx
    ON dx.EncounterId = e.EncounterId;


SELECT *
FROM vw_Clinical;





CREATE OR ALTER VIEW vw_Analytics_DeId
AS
SELECT
    e.EncounterId,
    CASE
        WHEN DATEDIFF(YEAR, pt.DateOfBirth, GETDATE()) < 18 THEN '0-17'
        WHEN DATEDIFF(YEAR, pt.DateOfBirth, GETDATE()) < 65 THEN '18-64'
        ELSE '65+'
    END AS AgeBand,
    pt.Gender,
    e.EncounterType,
    e.DepartmentId
FROM Patient pt
JOIN Encounter e
    ON e.PatientId = pt.PatientId;
