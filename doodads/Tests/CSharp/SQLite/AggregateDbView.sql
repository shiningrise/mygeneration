CREATE VIEW FullNameView AS
SELECT (LastName || ', ' || FirstName) AS 'FullName',
        DepartmentID,
        HireDate,
        Salary,
        IsActive
FROM AggregateTest
WHERE (((IsActive)=1));
