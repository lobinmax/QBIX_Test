SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[PositionDelete]
    @PositionUid UNIQUEIDENTIFIER
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
                WHERE e.PositionUid = @PositionUid
            )
    
        DELETE FROM PositionSkills
        WHERE PositionUid IN 
            (
                SELECT
                    p.PositionUid
                FROM Positions p
                WHERE p.PositionUid = @PositionUid
            )
    
        DELETE FROM Employees
        WHERE PositionUid = @PositionUid
    
        DELETE FROM Positions
        WHERE PositionUid = @PositionUid
        
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