SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[DeportamentDelete]
    @DeportamentUid UNIQUEIDENTIFIER
AS 
BEGIN
	SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION
 
        DELETE FROM EmployeeSkills
        WHERE EmployeeUid IN 
            (
                SELECT
                    e.EmployeeUid
                FROM Employees e
                WHERE e.DepartamentUid = @DeportamentUid
            )
    
        DELETE FROM PositionSkills
        WHERE PositionUid IN 
            (
                SELECT
                    p.PositionUid
                FROM Positions p
                WHERE p.DepartamenUid = @DeportamentUid
            )
    
        DELETE FROM Employees
        WHERE DepartamentUid = @DeportamentUid
    
        DELETE FROM Positions
        WHERE DepartamenUid = @DeportamentUid
    
        DELETE FROM Departaments
        WHERE DepartamentUid = @DeportamentUid
    
    IF (@@error != 0)
    BEGIN
    	ROLLBACK TRANSACTION
    END
    ELSE
    BEGIN
    	COMMIT TRANSACTION
    END

END
GO