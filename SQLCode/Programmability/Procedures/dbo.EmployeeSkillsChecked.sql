SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[EmployeeSkillsChecked]
    @PositionUid UNIQUEIDENTIFIER,
    @EmployeeUid UNIQUEIDENTIFIER
AS 
BEGIN
	
    SELECT 
        CAST(CASE WHEN es.EmployeeSkillUid IS NULL THEN 0 ELSE 1 END AS BIT) AS EmployeeSkillsBit, 
        ps.SkillUid, 
        ps.Name
    FROM PositionSkills ps
    LEFT JOIN EmployeeSkills es
        ON ps.SkillUid = es.SkillUid AND es.EmployeeUid = @EmployeeUid
    WHERE ps.PositionUid = @PositionUid 
    ORDER BY EmployeeSkillsBit DESC, ps.Name
END
GO