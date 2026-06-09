WITH OrderedEncounters AS (

    SELECT

        PatientId,
        EncounterId,
        AdmitDate,
        DischargeDate,

        LAG(DischargeDate)
            OVER (
                PARTITION BY PatientId
                ORDER BY AdmitDate
            ) AS PreviousDischarge

    FROM Encounter

    WHERE EncounterType = 'Inpatient'
)

SELECT

    PatientId,
    EncounterId,
    AdmitDate,
    PreviousDischarge,

    -- Number of days between visits

    DATEDIFF(
        DAY,
        PreviousDischarge,
        AdmitDate
    ) AS DaysBetweenVisits

FROM OrderedEncounters

WHERE PreviousDischarge IS NOT NULL

AND DATEDIFF(
        DAY,
        PreviousDischarge,
        AdmitDate
    ) <= 30;



SELECT

    e.EncounterId,

    d.Name AS Department,

    -- Length of stay for this encounter

    DATEDIFF(
        DAY,
        e.AdmitDate,
        e.DischargeDate
    ) AS LengthOfStay,

    -- Average LOS for all encounters
    -- in the same department

    AVG(
        DATEDIFF(
            DAY,
            e.AdmitDate,
            e.DischargeDate
        )
    )
    OVER (
        PARTITION BY e.DepartmentId
    ) AS DepartmentAverageLOS

FROM Encounter e

JOIN Department d
    ON d.DepartmentId = e.DepartmentId

WHERE e.DischargeDate IS NOT NULL;

SELECT

    p.FullName,

    COUNT(e.EncounterId) AS EncounterCount,

    RANK()
    OVER (
        ORDER BY COUNT(e.EncounterId) DESC
    ) AS VolumeRank

FROM Provider p

LEFT JOIN Encounter e
    ON e.ProviderId = p.ProviderId

GROUP BY
    p.ProviderId,
    p.FullName;

select EncounterType,AdmitDate,DischargeDate from [dbo].[Encounter]
order by AdmitDate;


