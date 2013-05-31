CREATE TABLE [dbo].[AggregateTest] ( 
    [ID]          	INT IDENTITY(1,1) NOT NULL,
    [DepartmentID]	INT NULL,
    [FirstName]   	VARCHAR(25) NULL,
    [LastName]    	VARCHAR(15) NULL,
    [Age]         	INT NULL,
    [HireDate]    	DATETIME NULL,
    [Salary]      	NUMERIC(8,4) NULL,
    [IsActive]    	BIT NULL,
    CONSTRAINT [PK_AggregateTest] PRIMARY KEY([ID])
)
GO
