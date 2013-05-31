
-----------------------------------------------------------------------------
-- DDL for Table 'pubs2.dbo.DataTypeTest'
-----------------------------------------------------------------------------
print '<<<<< CREATING Table - "pubs2.dbo.DataTypeTest" >>>>>'
go

use pubs2
go 

setuser 'dbo'
go 

create table DataTypeTest (
	Field_Int                       int                              not null  ,
	Field_BigInt                    bigint                           not null  ,
	Field_Binary                    binary(128)                          null  ,
	Field_Bit                       bit                              not null  ,
	Field_Char                      char(23)                             null  ,
	Field_Date                      date                             not null  ,
	Field_DateTime                  datetime                         not null  ,
	Field_Decimal                   decimal(14,3)                        null  ,
	Field_DoublePrecision           float(16)                        not null  ,
	Field_Float                     float(8)                         not null  ,
	Field_ID                        id                               not null  ,
	Field_Image                     image                            not null  ,
	Field_LongSysName               longsysname                      not null  ,
	Field_Money                     money                            not null  ,
	Field_NChar                     nchar(12)                        not null  ,
	Field_Numeric                   numeric(6,2)                         null  ,
	Field_NVarChar                  nvarchar(15)                     not null  ,
	Field_Real                      real                             not null  ,
	Field_SmallDateTime             smalldatetime                    not null  ,
	Field_SmallInt                  smallint                             null  ,
	Field_SmallMoney                smallmoney                       not null  ,
	Field_SysName                   sysname(30)                          null  ,
	Field_Text                      text                             not null  ,
	Field_Tid                       tid                                  null  ,
	Field_Time                      time                             not null  ,
	Field_TimeStamp                 timestamp                        not null  ,
	Field_TinyInt                   tinyint                              null  ,
	Field_UniChar                   unichar(64)                      not null  ,
	Field_UniText                   unitext                          not null  ,
	Field_UniVarChar                univarchar(23)                   not null  ,
	Field_UnsignedBigInt            unsigned bigint                  not null  ,
	Field_UnsignedInt               unsigned int                     not null  ,
	Field_UnsignedSmallInt          unsigned smallint                    null  ,
	Field_VarBinary                 varbinary(65)                    not null  ,
	Field_VarChar                   varchar(34)                      not null  ,
	Field_Int_Identity              int                              identity  ,
	Field_Int_Computed              AS  Field_Int + 13   
)
lock allpages
 on 'default'
go 

sp_placeobject 'default', 'dbo.DataTypeTest.tDataTypeTest'
go 


-----------------------------------------------------------------------------
-- DDL for Table 'pubs2.dbo.Region'
-----------------------------------------------------------------------------
print '<<<<< CREATING Table - "pubs2.dbo.Region" >>>>>'
go

use pubs2
go 

setuser 'dbo'
go 

IF EXISTS (SELECT 1 FROM sysobjects o, sysusers u WHERE o.uid=u.uid AND o.name = 'Region' AND u.name = 'dbo' AND o.type = 'U')
	drop table Region

IF (@@error != 0)
BEGIN
	PRINT "Error CREATING table 'pubs2.dbo.Region'"
	SELECT syb_quit()
END
go

create table Region (
	RegionID                        int                              not null  ,
	RegionDescription               nchar(50)                        not null  ,
		CONSTRAINT PK_Region PRIMARY KEY NONCLUSTERED ( RegionID )  on 'default' 
)
lock allpages
 on 'default'
go 


setuser
go 



-----------------------------------------------------------------------------

-- DDL for Table 'pubs2.dbo.Territories'

-----------------------------------------------------------------------------

print '<<<<< CREATING Table - "pubs2.dbo.Territories" >>>>>'

go



use pubs2

go 



setuser 'dbo'

go 



IF EXISTS (SELECT 1 FROM sysobjects o, sysusers u WHERE o.uid=u.uid AND o.name = 'Territories' AND u.name = 'dbo' AND o.type = 'U')

	drop table Territories



IF (@@error != 0)

BEGIN

	PRINT "Error CREATING table 'pubs2.dbo.Territories'"

	SELECT syb_quit()

END

go



create table Territories (

	TerritoryID                     nvarchar(20)                     not null  ,

	TerritoryDescription            nchar(50)                        not null  ,

	RegionID                        int                              not null  ,

		CONSTRAINT PK_Territories PRIMARY KEY NONCLUSTERED ( TerritoryID )  on 'default' 

)

lock allpages

 on 'default'

go 





setuser

go 



-----------------------------------------------------------------------------

-- Dependent DDL for Object(s)

-----------------------------------------------------------------------------

alter table pubs2.dbo.Territories

add constraint FK_Territories_Region FOREIGN KEY (RegionID) REFERENCES pubs2.dbo.Region(RegionID)

go



IF EXISTS (SELECT 1 FROM sysobjects o, sysusers u WHERE o.uid=u.uid AND o.name = 'CompositeKeyTest' AND u.name = 'dbo' AND o.type = 'U')
	drop table CompositeKeyTest

IF (@@error != 0)
BEGIN
	PRINT "Error CREATING table 'pubs2.dbo.CompositeKeyTest'"
	SELECT syb_quit()
END
go

create table CompositeKeyTest (
	GroupID                        int                              not null  ,
	IndyID                        int                              not null  ,
	IndyDescription               nchar(50)                        not null  ,
		CONSTRAINT PK_CompositeKeyTest PRIMARY KEY NONCLUSTERED ( GroupID, IndyID )  on 'default' 
)
lock allpages
 on 'default'
go 


setuser
go 

IF EXISTS (SELECT 1 FROM sysobjects o, sysusers u WHERE o.uid=u.uid AND o.name = 'CompositeKeyReferenceTest' AND u.name = 'dbo' AND o.type = 'U')
	drop table CompositeKeyReferenceTest

IF (@@error != 0)
BEGIN
	PRINT "Error CREATING table 'pubs2.dbo.CompositeKeyReferenceTest'"
	SELECT syb_quit()
END
go

create table CompositeKeyReferenceTest (
	RefPKID                        int                              not null  ,
	GroupID                        int                              not null  ,
	IndyID                        int                              not null  ,
	IndyDescription               nchar(50)                        not null  ,
		CONSTRAINT PK_CompositeKeyReferenceTest PRIMARY KEY NONCLUSTERED ( RefPKID )  on 'default' 
)
lock allpages
 on 'default'
go 


setuser
go

alter table pubs2.dbo.CompositeKeyReferenceTest

add constraint FK_CompositeKeyReferenceTest_CompositeKeyTest FOREIGN KEY (GroupID, IndyID) REFERENCES pubs2.dbo.CompositeKeyTest(GroupID, IndyID)

go

