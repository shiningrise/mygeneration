CREATE TABLE [AggregateTest] ( 
    [ID]          	INTEGER NOT NULL,
    [DepartmentID]	INTEGER,
    [FirstName]   	VARCHAR(25),
    [LastName]    	VARCHAR(15),
    [Age]         	INTEGER,
    [HireDate]    	DATETIME,
    [Salary]      	NUMERIC(8,4),
    [IsActive]    	BOOLEAN,
    PRIMARY KEY(ID)
);
