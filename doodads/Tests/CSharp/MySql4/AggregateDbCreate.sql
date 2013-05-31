CREATE TABLE AggregateTest (
    ID          	INT NOT NULL AUTO_INCREMENT,
    DepartmentID	INT,
    FirstName   	VARCHAR(25),
    LastName    	VARCHAR(15),
    Age         	INTEGER,
    HireDate    	DATETIME,
    Salary      	NUMERIC(8,4),
    IsActive    	TINYINT(1),
    PRIMARY KEY(ID)
);
