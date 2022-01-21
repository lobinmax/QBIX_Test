SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[EmployeeDelete]
    @EmployeeUid UNIQUEIDENTIFIER
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
                WHERE e.EmployeeUid = @EmployeeUid
            )

        DELETE FROM Employees
        WHERE EmployeeUid = @EmployeeUid
            
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