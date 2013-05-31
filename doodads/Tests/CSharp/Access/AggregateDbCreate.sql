CREATE TABLE AggregateTest (
    [ID]            AUTOINCREMENT  NOT NULL,
    [DepartmentID]  INTEGER,
    [FirstName]     TEXT(25),
    [LastName]      TEXT(15),
    [Age]           INTEGER,
    [HireDate]      DATETIME,
    [Salary]        NUMERIC,
    [IsActive]      BIT,
    CONSTRAINT [PK_AggregateTest] PRIMARY KEY([ID])
);
