CREATE VIEW [dbo].[FullNameView] AS 
SELECT (AggregateTest.LastName + ', ' + AggregateTest.FirstName) AS 'FullName',
        AggregateTest.DepartmentID,
        AggregateTest.HireDate,
        AggregateTest.Salary,
        AggregateTest.IsActive
FROM AggregateTest
WHERE (((AggregateTest.IsActive)=1))
GO
