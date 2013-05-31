CREATE TABLE "AggregateTest"
(
  "ID" int4 NOT NULL DEFAULT nextval('public."AggregateTest_ID_seq"'::text),
  "DepartmentID" int4,
  "LastName" varchar(25),
  "FirstName" varchar(15),
  "Age" int4,
  "HireDate" timestamp,
  "Salary" numeric(8,4),
  "IsActive" bool,
  CONSTRAINT "OK_ID" PRIMARY KEY ("ID")
) 
WITHOUT OIDS;
ALTER TABLE "AggregateTest" OWNER TO postgres;