SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vEmployeesDs] 
AS SELECT
    e.EmployeeUid
   ,e.DepartamentUid
   ,e.Surname + ' ' + e.Name + ' ' + e.Patronymic AS SNP
   ,p.Name AS PositionName
   ,e.DtHired
   ,e.DtDismissed
FROM Employees e
INNER JOIN Departaments d
    ON e.DepartamentUid = d.DepartamentUid
INNER JOIN Positions p 
    ON e.PositionUid = p.PositionUid
GO